// Copyright 2012 Max Toro Q.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using myxsl.common;

namespace myxsl.schematron {
   
   public class SchematronResultHandler {

      readonly SchematronValidator validator;
      readonly SchematronRuntimeOptions options;
      readonly XPathSerializationOptions defaultSerialization;

      internal SchematronResultHandler(SchematronValidator validator, SchematronRuntimeOptions options) {
         
         this.validator = validator;
         this.options = options;
         this.defaultSerialization = options.Serialization;
      }

      public bool IsValid() {

         XPathNavigator doc = ToDocument().CreateNavigator();
         doc.MoveToChild(XPathNodeType.Element);

         return !doc.MoveToChild( "failed-assert", SchematronInvoker.SvrlNamespace);
      }

        public (bool IsValid,List<SchematronFailedAssert> FailedAsserts) GetValidationResult()
        {
    
            var result = new List<SchematronFailedAssert>();
            XPathNavigator doc = ToDocument().CreateNavigator();
            doc.MoveToChild(XPathNodeType.Element);
            var failedAssertNavigators = doc.SelectDescendants("failed-assert", SchematronInvoker.SvrlNamespace,false)?.Cast<XPathNavigator>()?.ToList();
            
            foreach (var item in failedAssertNavigators)
            {
                var fa = new SchematronFailedAssert()
                {
                    Id = item.GetAttribute("id", String.Empty),
                    Test = item.GetAttribute("test", String.Empty),
                    Location = item.GetAttribute("location", String.Empty),
                    Role = item.GetAttribute("role", String.Empty),
                    Flag = item.GetAttribute("flag", String.Empty),
                    
                };

                var text = item.SelectChildren("text", SchematronInvoker.SvrlNamespace)?.Cast<XPathNavigator>().ToList();
                fa.Text = text?.FirstOrDefault()?.Value;


                var drs = item.SelectChildren("diagnostic-reference", SchematronInvoker.SvrlNamespace)?.Cast<XPathNavigator>().ToList();
                if (drs?.Count > 0)
                {
                    fa.DiagnosticReferences = new List<DiagnosticReference>();
                    foreach (var drn in drs)
                    {
                        var dr = new DiagnosticReference()
                        {
                            Diagnostic = drn.GetAttribute("diagnostic", String.Empty)
                        };
                        var drtext = drn.SelectChildren("text", SchematronInvoker.SvrlNamespace)?.Cast<XPathNavigator>().ToList();
                        dr.Text = drtext?.FirstOrDefault()?.Value;
                        fa.DiagnosticReferences.Add(dr);
                    }
                }

                result.Add(fa);
            }
            return (result.Count == 0, result);
        }


        public IXPathNavigable ToDocument() {

         IXPathNavigable doc = this.validator.ItemFactory.BuildNode();

         XmlWriter writer = doc.CreateNavigator().AppendChild();
         
         To(writer);

         writer.Close();

         return doc;
      }

       


        public void To(Stream output) {
         To(output, null);
      }

      public void To(Stream output, XPathSerializationOptions options) {

         OverrideSerialization(options);

         this.validator.Validate(output, this.options);

         RestoreSerialization(options);
      }

      public void To(TextWriter output) {
         To(output, null);
      }

      public void To(TextWriter output, XPathSerializationOptions options) {

         OverrideSerialization(options);

         this.validator.Validate(output, this.options);

         RestoreSerialization(options);
      }

      public void To(XmlWriter output) {
         To(output, null);
      }

      public void To(XmlWriter output, XPathSerializationOptions options) {

         OverrideSerialization(options);

         this.validator.Validate(output, this.options);

         RestoreSerialization(options);
      }

      void OverrideSerialization(XPathSerializationOptions options) {

         if (options == null) {
            return;
         }

         this.options.Serialization = options;
      }

      void RestoreSerialization(XPathSerializationOptions options) {

         if (options == null) {
            return;
         }

         this.options.Serialization = this.defaultSerialization;
      }
   }
}
