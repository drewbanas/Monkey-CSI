namespace repl
{
    using System.Collections.Generic;

    using lexer;
    using parser;
    using ast;
    //using Object;
    //using evaluator;

    class repl
    {

        const string PROMPT = ">> ";

        public static void Start()
        {
            Object.Environment env = Object.Environment.NewEnvironment();
            Object.Environment macroEnv = Object.Environment.NewEnvironment();

            for (;;)
            {
                System.Console.Write(PROMPT);
                string line = System.Console.ReadLine();

                lexer.Lexer l = lexer.Lexer.New(line);
                parser.Parser p = parser.Parser.New(l);

                ast.Program program = p.ParseProgram();
                if (parser.Parser.Errors().Count != 0)
                {
                    printParserErrors(parser.Parser.Errors());
                    continue;
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

        /*
         * Wonderful artwork generated using:
         * https://tomeko.net/online_tools/cpp_text_escape.php?lang=en
        */
        const string MONKEY_FACE = "            __,__\n   .--.  .-\"     \"-.  .--.\n  / .. \\/  .-. .-.  \\/ .. \\\n | |  '|  /   Y   \\  |'  | |\n | \\   \\  \\ 0 | 0 /  /   / |\n  \\ '- ,\\.-\"\"\"\"\"\"\"-./, -' /\n   ''-' /_   ^ ^   _\\ '-''\n       |  \\._   _./  |\n       \\   \\ '~' /   /\n        '._ '-=-' _.'\n           '-----'";

        public static void printParserErrors(List<string> errors)
        {
            System.Console.WriteLine(MONKEY_FACE);
            System.Console.WriteLine("Woops! We ran into some monkey business here!");
            System.Console.WriteLine(" parse errors:");
            foreach (string msg in errors)
            {
                System.Console.WriteLine("\t" + msg);
            }
        }
    }
}
