namespace monkey
{
    using repl;

    class main
    {
        static void Main(string[] args)
        {

            if (args.Length == 0) // repl
            {
                System.Console.WriteLine("\nHello {0}! This is the Monkey programming language!\n", System.Environment.UserName);
                System.Console.WriteLine("Feel free to type in comands\n");
                repl.Start();
            }
            else // script file
            {
                runFile(args[0]);
            }
#if DEBUG
            System.Console.ReadKey();
#endif
        }

        /*
         * EXTRA STUFF TO RUN SCRIPT FILES
         */

        // taken from CLOXCS
        static string readFile(string path)
        {
            System.Text.StringBuilder buffer = null;
            if (!System.IO.File.Exists(path))
            {
                System.Console.WriteLine("Could not open file {0}.", path);
                System.Environment.Exit(74);
            }
            buffer = new System.Text.StringBuilder(System.IO.File.ReadAllText(path));
            buffer.Append('\0');
            if (buffer == null)
            {
                System.Console.WriteLine("Not enough memory to read {0}.", path);
                System.Environment.Exit(74);
            }
            return buffer.ToString();
        }

        private static void runFile(string path) // REPL without the loop
        {
            Object.Environment env = Object.Environment.NewEnvironment();
            Object.Environment macroEnv = Object.Environment.NewEnvironment();

            string source = readFile(path);
            lexer.Lexer l = lexer.Lexer.New(source);
            parser.Parser p = parser.Parser.New(l);

            ast.Program program = p.ParseProgram();
            if (parser.Parser.Errors().Count != 0)
            {
                repl.printParserErrors(parser.Parser.Errors());
                System.Environment.Exit(77);
            }

            evaluator.macro_expansion.DefineMacros(program, macroEnv);
            ast.Node expanded = evaluator.macro_expansion.ExpandMacros(program, macroEnv);

            Object.Object evaluated = evaluator.evaluator.Eval(expanded, env);
            if (evaluated != null)
            {
                System.Console.Write(evaluated.Inspect());
                System.Console.WriteLine();
            }
        }
    }
}
