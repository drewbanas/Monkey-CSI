namespace evaluator
{
    using System.Collections.Generic;

    class macro_expansion
    {
        public static void DefineMacros(ast.Program program, Object.Environment env)
        {
            List<int> defenitions = new List<int>();

            for(int i = 0; i < program.Statements.Count; i++)
            {
                ast.Statement statement = program.Statements[i];
                if (isMacroDefenition(statement))
                {
                    addMacro(statement, env);
                    defenitions.Add(i++);
                }
            }

            for (int i = defenitions.Count - 1; i >= 0; i--)
            {
                int defenitionIndex = defenitions[i];
                program.Statements.RemoveAt(defenitionIndex);
            }
        }

        static bool isMacroDefenition(ast.Statement node)
        {
            if (!(node is ast.LetStatement))
            {
                return false;
            }
            ast.LetStatement letStatement = (ast.LetStatement)node;

            if (!(letStatement.Value is ast.MacroLiteral))
            {
                return false;
            }

            return true;

        }

        static void addMacro(ast.Statement stmt, Object.Environment env)
        {
            ast.LetStatement letStatement = (ast.LetStatement)stmt;
            ast.MacroLiteral macroLiteral = (ast.MacroLiteral)letStatement.Value;

            Object.Macro macro = new Object.Macro
            {
                Parameters = macroLiteral.Parameters,
                Env = env,
                Body = macroLiteral.Body
            };

            env.Set(letStatement.Name.Value, macro);
        }

        public static ast.Node ExpandMacros(ast.Node program, Object.Environment env)
        {
            _env = env;
            return ast.modify.Modify(program, _ExpandMacros);
        }

        private static Object.Environment _env;
        static ast.Node _ExpandMacros(ast.Node node)
        {
            if (!(node is ast.CallExpression))
            {
                return node;
            }
            ast.CallExpression callExpression = (ast.CallExpression)node;

            Object.Macro macro = isMacroCall(callExpression, _env);
            if (macro == null)
            {
                return node;
            }

            List<Object.Quote> args = quoteArgs(callExpression);
            Object.Environment evalEnv = extendMacroEnv(macro, args);

            Object.Object evaluated = evaluator.Eval(macro.Body, evalEnv);
            if (!(evaluated is Object.Quote))
            {
                System.Console.WriteLine("we only support returning AST-nodes from macros");
                System.Environment.Exit(-1);
            }
            Object.Quote quote = (Object.Quote)evaluated;

            return quote.Node;
        }

        static Object.Macro isMacroCall(ast.CallExpression exp, Object.Environment env)
        {
            if (!(exp.Function is ast.Identifier))
            {
                return null;
            }
            ast.Identifier identifier = (ast.Identifier)exp.Function;

            Object.Object obj = env.Get(identifier.Value);
            if (obj == null)
            {
                return null;
            }

            if (!(obj is Object.Macro))
            {
                return null;
            }
            Object.Macro macro = (Object.Macro)obj;

            return macro;
        }

        static List<Object.Quote> quoteArgs(ast.CallExpression exp)
        {
            List<Object.Quote> args = new List<Object.Quote>();

            foreach (ast.Expression a in exp.Arguments)
            {
                args.Add(new Object.Quote { Node = a });
            }

            return args;
        }

        static Object.Environment extendMacroEnv(Object.Macro macro, List<Object.Quote> args)
        {
            Object.Environment extended = Object.Environment.NewEnclosedEnvironment(macro.Env);

            int paramIdx = 0;
            foreach(ast.Identifier param in macro.Parameters)
            {
                extended.Set(param.Value, args[paramIdx++]);
            }

            return extended;
        }
    }
}
