namespace evaluator
{
    class quote_unquote
    {
        public static Object.Object quote(ast.Node node, Object.Environment env)
        {
            node = evalUnquoteCalls(node, env);
            return new Object.Quote { Node = node };
        }

        public static ast.Node evalUnquoteCalls(ast.Node quoted, Object.Environment env)
        {
            _env = env;
            return ast.modify.Modify(quoted, _evalUnquotedCalss);
        }

        private static Object.Environment _env;  // to pass the environment on the originally crammed-to-argument function definition
        static ast.Node _evalUnquotedCalss(ast.Node node)
        {
            if (!isUnquoteCall(node))
            {
                return node;
            }

            if (!(node is ast.CallExpression))
            {
                return node;
            }
            ast.CallExpression call = (ast.CallExpression)node;

            if (call.Arguments.Count != 1)
            {
                return node;
            }

            Object.Object unquoted = evaluator.Eval(call.Arguments[0], _env); // env needs to reach this
            return convertObjectToASTNode(unquoted);
        }

        static bool isUnquoteCall(ast.Node node)
        {
            if (!(node is ast.CallExpression))
            {
                return false;
            }
            ast.CallExpression callExpression = (ast.CallExpression)node;

            return callExpression.Function.TokenLiteral() == "unquote";
        }

        static ast.Node convertObjectToASTNode(Object.Object obj)
        {
            if (obj is Object.Integer)
            {
                Object.Integer _obj = (Object.Integer)obj;
                token.Token t = new token.Token
                {
                    Type = token.token.INT,
                    Literal = _obj.Value.ToString()
                };
                return new ast.IntegerLiteral { Token = t, Value = _obj.Value };
            }


            if (obj is Object.Boolean)
            {
                Object.Boolean _obj = (Object.Boolean)obj;
                token.Token t;
                if (_obj.Value)
                {
                    t = new token.Token { Type = token.token.TRUE, Literal = "true" };
                }
                else
                {
                    t = new token.Token { Type = token.token.FALSE, Literal = "false" };
                }
                return new ast.Boolean { Token = t, Value = _obj.Value };
            }

            if (obj is Object.Quote)
                return ((Object.Quote)obj).Node;

            // default:
            return null;
        }
    }
}
