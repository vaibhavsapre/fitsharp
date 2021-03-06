﻿// Copyright © 2009 Syterra Software Inc.
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 2.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

using System.Collections.Generic;
using System.Reflection;
using fit.Operators;
using fitSharp.Fit.Operators;
using fitSharp.Fit.Service;
using fitSharp.Machine.Application;
using fitSharp.Machine.Engine;
using fitSharp.Machine.Model;

namespace fit.Service {
    public class Service: CellProcessorBase, Copyable {
        public Service() {
            AddOperator(new RuntimeFlow());
            AddOperator(new RuntimeProcedure());

            AddOperator(new ComposeDefault());

            AddOperator(new ComposeStoryTestString());
            AddOperator(new ParseStoryTestString());

            AddOperator(new ComposeTable());
            AddOperator(new ExecuteList());
            AddOperator(new ParseTable());
            AddOperator(new ParseTree());
            AddOperator(new ParseInterpreter());

            ApplicationUnderTest = Context.Configuration.GetItem<ApplicationUnderTest>();
            ApplicationUnderTest.AddNamespace("fit");
            ApplicationUnderTest.AddNamespace("fitnesse.handlers");
            ApplicationUnderTest.AddNamespace("fit.Operators");
            ApplicationUnderTest.AddNamespace("fitSharp.Fit.Fixtures");
            ApplicationUnderTest.AddNamespace("fitSharp.Fit.Operators");
            ApplicationUnderTest.AddAssembly(Assembly.GetExecutingAssembly().CodeBase);
        }

        public Service(Service other): base(other) {}

        public void AddCellHandler(string handlerName) {
            if (renames.ContainsKey(handlerName.ToLower())) AddOperator(renames[handlerName.ToLower()]);
        }

        public void RemoveCellHandler(string handlerName) {
            if (renames.ContainsKey(handlerName.ToLower())) RemoveOperator(renames[handlerName.ToLower()]);
        }

        private static readonly Dictionary<string, string> renames = new Dictionary<string, string> {
            {"boolhandler", typeof(ParseBoolean).FullName},
            {"emptycellhandler", typeof(ExecuteEmpty).FullName},
            {"exceptionkeywordhandler", typeof(ExecuteException).FullName},
            {"nullkeywordhandler", typeof(ParseNull).FullName},
            {"blankkeywordhandler", typeof(ParseBlank).FullName},
            {"errorkeywordhandler", typeof(ExecuteError).FullName},
            {"endswithhandler", typeof(CompareEndsWith).FullName},
            {"failkeywordhandler", typeof(CompareFail).FullName},
            {"startswithhandler", typeof(CompareStartsWith).FullName},
            {"integralrangehandler", typeof(CompareIntegralRange).FullName},
            {"listhandler", typeof(ExecuteList).FullName},
            {"numericcomparehandler", typeof(CompareNumeric).FullName},
            {"parsecellhandler", typeof(ExecuteParse).FullName},
            {"stringhandler", typeof(CompareString).FullName},
            {"substringhandler", typeof(CompareSubstring).FullName},
            {"symbolsavehandler", typeof(ExecuteSymbolSave).FullName},
            {"symbolrecallhandler", typeof(ParseSymbol).FullName},
            {"tablehandler", typeof(ParseTable).FullName},
            {"regexhandler", typeof(CompareRegEx).FullName}
        };

        Copyable Copyable.Copy() {
            return new Service(this);
        }
    }
}