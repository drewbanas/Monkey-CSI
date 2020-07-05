namespace ast
{
    using System.Collections.Generic;

    struct modify
    {
        public delegate ast.Node MofifierFunc(ast.Node node);

        public static ast.Node Modify(ast.Node node, MofifierFunc modifier)
        {
            if (node is ast.Program)
            {
                ast.Program _node = (ast.Program)node;
                for (int i = 0; i < _node.Statements.Count; i++)
                {
                    _node.Statements[i] = (ast.Statement)Modify(_node.Statements[i], modifier);
                }
                node = _node;
            }

            if (node is ast.ExpressionStatement)
            {
                ast.ExpressionStatement _node = (ast.ExpressionStatement)node;
                _node.Expression = (ast.Expression)Modify(_node.Expression, modifier);
                node = _node;
            }

            if (node is ast.InfixExpression)
            {
                ast.InfixExpression _node = (ast.InfixExpression)node;
                _node.Left = (ast.Expression)Modify(_node.Left, modifier);
                _node.Right = (ast.Expression)Modify(_node.Right, modifier);
                node = _node;
            }

            if (node is ast.PrefixExpression)
            {
                ast.PrefixExpression _node = (ast.PrefixExpression)node;
                _node.Right = (ast.Expression)Modify(_node.Right, modifier);
                node = _node;
            }

            if (node is ast.IndexExpression)
            {
                ast.IndexExpression _node = (ast.IndexExpression)node;
                _node.Left = (ast.Expression)Modify(_node.Left, modifier);
                _node.Index = (ast.Expression)Modify(_node.Index, modifier);
                node = _node;
            }

            if (node is ast.IfExpression)
            {
                ast.IfExpression _node = (ast.IfExpression)node;
                _node.Condition = (ast.Expression)Modify(_node.Condition, modifier);
                _node.Consequence = (ast.BlockStatement)Modify(_node.Consequence, modifier);
                node = _node;
                if (_node.Alternative != null)
                {
                    _node.Alternative = (ast.BlockStatement)Modify(_node.Alternative, modifier);
                }
            }

            if (node is ast.BlockStatement)
            {
                ast.BlockStatement _node = (ast.BlockStatement)node;
                for (int i = 0; i < _node.Statements.Count; i++)
                {
                    _node.Statements[i] = (ast.Statement)Modify(_node.Statements[i], modifier);
                }
                node = _node;
            }


            if (node is ast.ReturnStatement)
            {
                ast.ReturnStatement _node = (ast.ReturnStatement)node;
                _node.ReturnValue = (ast.Expression)Modify(_node.ReturnValue, modifier);
                node = _node;
            }

            if (node is ast.LetStatement)
            {
                ast.LetStatement _node = (ast.LetStatement)node;
                _node.Value = (ast.Expression)Modify(_node.Value, modifier);
                node = _node;
            }

            if (node is ast.FunctionLiteral)
            {
                ast.FunctionLiteral _node = (ast.FunctionLiteral)node;
                for (int i = 0; i < _node.Parameters.Count; i++)
                {
                    _node.Parameters[i] = (ast.Identifier)Modify(_node.Parameters[i], modifier);
                }
                _node.Body = (ast.BlockStatement)Modify(_node.Body, modifier);
                node = _node;
            }

            if (node is ast.ArrayLiteral)
            {
                ast.ArrayLiteral _node = (ast.ArrayLiteral)node;
                for (int i = 0; i < _node.Elements.Count; i++)
                {
                    _node.Elements[i] = (ast.Expression)Modify(_node.Elements[i], modifier);
                }
                node = _node;
            }

            if (node is ast.HashLiteral)
            {
                ast.HashLiteral _node = (ast.HashLiteral)node;
                Dictionary<ast.Expression, ast.Expression> newPairs = new Dictionary<ast.Expression, ast.Expression>();
                foreach (KeyValuePair<ast.Expression, ast.Expression> kv in _node.Pairs)
                {
                    ast.Expression newKey = (ast.Expression)Modify(kv.Key, modifier);
                    ast.Expression newVal = (ast.Expression)Modify(kv.Value, modifier);
                    newPairs.Add(newKey, newVal);
                }
                _node.Pairs = newPairs;
                node = _node;
            }

            return modifier(node);
        }

    }
}
