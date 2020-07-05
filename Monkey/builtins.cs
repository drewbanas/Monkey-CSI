namespace evaluator
{
    using System.Collections.Generic;
    //using Object;

    class builtins_
    {
        public static Dictionary<string, Object.Builtin> builtins = new Dictionary<string, Object.Builtin>
        {
            {"len" , new Object.Builtin {Fn = len}},
            {"first" , new Object.Builtin {Fn = first}},
            {"last" , new Object.Builtin {Fn = last}},
            {"rest" , new Object.Builtin {Fn = rest}},
            {"push" , new Object.Builtin {Fn = push}},
            {"puts" , new Object.Builtin {Fn = puts}}
        };


        static Object.Object len(List<Object.Object> args) // used List<> instead of params
        {
            if (args.Count != 1)
            {
                return evaluator.newError("wrong number of arguments. got={0}, want={1}", args.Count.ToString());
            }

            Object.Object arg = args[0];
            if (arg is Object.Array)
                return new Object.Integer { Value = (long)((Object.Array)arg).Elements.Count };
            if (arg is Object.String)
                return new Object.Integer { Value = (long)((Object.String)arg).Value.Length };

            // default:
            return evaluator.newError("argument to 'len' not supported, got {0}", args[0].Type());
        }

        static Object.Object puts(List<Object.Object> args)
        {
            foreach (Object.Object arg in args)
            {
                System.Console.WriteLine(arg.Inspect());
            }

            return evaluator.NULL;
        }

        static Object.Object first(List<Object.Object> args)
        {
            if (args.Count != 1)
            {
                return evaluator.newError("wrong number of arguments. got={0}, want=1", args.Count.ToString());
            }

            if (args[0].Type() != Object._ObjectType.ARRAY_OBJ)
            {
                return evaluator.newError("argument to 'first' must be ARRAY. got={0}", args[0].Type());
            }

            Object.Array arr = (Object.Array)args[0];
            if (arr.Elements.Count > 0)
            {
                return arr.Elements[0];
            }

            return evaluator.NULL;
        }

        static Object.Object last(List<Object.Object> args)
        {
            if (args.Count != 1)
            {
                return evaluator.newError("wrong number of arguments. got={0}, want=1", args.Count.ToString());
            }

            if (args[0].Type() != Object._ObjectType.ARRAY_OBJ)
            {
                return evaluator.newError("argument to 'last' must be ARRAY. got={0}", args[0].Type());
            }

            Object.Array arr = (Object.Array)args[0];
            int length = arr.Elements.Count;
            if (length > 0)
            {
                return arr.Elements[length - 1];
            }

            return evaluator.NULL;
        }

        static Object.Object rest(List<Object.Object> args)
        {
            if (args.Count != 1)
            {
                return evaluator.newError("wrong number of arguments. got={0}, want=1", args.Count.ToString());
            }

            if (args[0].Type() != Object._ObjectType.ARRAY_OBJ)
            {
                return evaluator.newError("argument to 'rest' must be ARRAY. got={0}", args[0].Type());
            }

            Object.Array arr = (Object.Array)args[0];
            int length = arr.Elements.Count;
            if (length > 0)
            {
                List<Object.Object> newElements = new List<Object.Object>(new Object.Object[length - 1]);

                for (int i = 1; i < length; i++)
                    newElements[i - 1] = arr.Elements[i];

                return new Object.Array { Elements = newElements };
            }

            return evaluator.NULL;
        }

        static Object.Object push(List<Object.Object> args)
        {
            if (args.Count != 2)
            {
                return evaluator.newError("wrong number of arguments. got={0}, want=2", args.Count.ToString());
            }

            if (args[0].Type() != Object._ObjectType.ARRAY_OBJ)
            {
                return evaluator.newError("argument to 'push' must be ARRAY. got={0}", args[0].Type());
            }

            Object.Array arr = (Object.Array)args[0];
            int length = arr.Elements.Count;

            List<Object.Object> newElements = new List<Object.Object>(new Object.Object[length + 1]);
            for (int i = 0; i < length; i++)
                newElements[i] = arr.Elements[i];
            newElements[length] = args[1];

            return new Object.Array { Elements = newElements };
        }

    }
}
