﻿// Copyright 2010 Max Toro Q.
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
using System.Reflection;
using System.Collections.ObjectModel;

namespace myxsl.common {

   public static class XPathModules {

      static ReadOnlyCollection<XPathModuleInfo> _Modules;
      static readonly object padlock = new object();

      public static ReadOnlyCollection<XPathModuleInfo> Modules {
         get {
            if (_Modules == null) {
               lock (padlock) {
                  if (_Modules == null) {

                     List<Assembly> assemblies = TypeLoader.Instance
                        .GetReferencedAssemblies()
                        .ToList();

                     Assembly thisAssembly = typeof(XPathModules).Assembly;

                     if (assemblies.Find(a => Object.ReferenceEquals(a, thisAssembly)) == null) {
                        assemblies.Add(thisAssembly);
                     }

                     IEnumerable<XPathModuleInfo> exportedModules =
                        from a in assemblies
                        where a.IsDefined(typeof(XPathModuleExportAttribute), inherit: true)
                        from t in a.GetTypes()
                        where t.IsDefined(typeof(XPathModuleAttribute), inherit: true)
                        select new XPathModuleInfo(t);

                     _Modules = new ReadOnlyCollection<XPathModuleInfo>(exportedModules.ToArray());
                  }
               }
            }
            return _Modules;
         }
      }
   }
}
