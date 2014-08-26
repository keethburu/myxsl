﻿// Copyright 2011 Max Toro Q.
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
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Xml;
using myxsl.common;
using myxsl.configuration;
using myxsl.web.compilation;
using myxsl.web.configuration;

namespace myxsl.web {

   public static class PreApplicationStartCode {

      static bool startWasCalled;

      public static void Start() {

         if (!startWasCalled) {

            startWasCalled = true;

            TypeLoader.Instance = new WebTypeLoader();

            ModifyConfig(LibraryConfigSection.Instance);

            BuildProvider.RegisterBuildProvider(".xsl", typeof(XsltPageBuildProvider));
            BuildProvider.RegisterBuildProvider(".xqy", typeof(XQueryPageBuildProvider));
         }
      }

      static void ModifyConfig(LibraryConfigSection config) {

         ResolverElement fileResolverConfig = config.Resolvers.Get(Uri.UriSchemeFile);
         ResolverElement httpResolverConfig = config.Resolvers.Get(Uri.UriSchemeHttp);

         if (fileResolverConfig != null
            && fileResolverConfig.TypeInternal == typeof(XmlUrlResolver)) {

            fileResolverConfig.TypeInternal = typeof(XmlVirtualPathAwareUrlResolver);
         }

         if (httpResolverConfig != null
            && httpResolverConfig.TypeInternal == typeof(XmlUrlResolver)) {

            httpResolverConfig.TypeInternal = typeof(XmlVirtualPathAwareUrlResolver);
         }
      }
   }
}