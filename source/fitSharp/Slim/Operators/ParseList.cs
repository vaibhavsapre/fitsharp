﻿// Copyright © 2009 Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using fitSharp.Machine.Engine;
using fitSharp.Machine.Extension;
using fitSharp.Machine.Model;

namespace fitSharp.Slim.Operators {
    public class ParseList: SlimOperator, ParseOperator<string> { // todo: handle any IList type
        public bool CanParse(Type type, TypedValue instance, Tree<string> parameters) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (List<>);
        }

        public TypedValue Parse(Type type, TypedValue instance, Tree<string> parameters) {
            return new TypedValue(
                parameters.Branches.AggregateTo(
                    (IList) Activator.CreateInstance(type),
                    (list, branch) => list.Add(Processor.ParseTree(type.GetGenericArguments()[0], branch).Value)));
        }
    }
}