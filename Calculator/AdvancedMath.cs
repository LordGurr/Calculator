using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    internal static class AdvancedMath
    {
        public static float Deg2Rad = ((float)Math.PI * 2) / 360;
        public static float Rad2Deg = 360 / ((float)Math.PI * 2);

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) // Obs Denna funktionen är tagen direkt från Unity där den är inbyggd. Jag skäms men jag behöver den för att göra någon dampening
        {
            smoothTime = Math.Max(0.0001f, smoothTime);
            float num = 2f / smoothTime;
            float num2 = num * deltaTime;
            float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
            float num4 = current - target;
            float num5 = target;
            float num6 = maxSpeed * smoothTime;
            num4 = Math.Clamp(num4, -num6, num6);
            target = current - num4;
            float num7 = (currentVelocity + num * num4) * deltaTime;
            currentVelocity = (currentVelocity - num * num7) * num3;
            float num8 = target + (num4 + num7) * num3;
            if (num5 - current > 0f == num8 > num5)
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

        public static float AngleBetween(Vector2 a, Vector2 B)
        {
            float dotProd;
            float Ratio;
            dotProd = Vector2.Dot(a, B);
            Ratio = dotProd / a.Length();
            return (float)(Math.Acos(Ratio)) * Rad2Deg;
        }

        public static float Vector2Distance(Vector2 a, Vector2 b)
        {
            return Magnitude(a - b);
        }

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 Rotate(Vector2 v, float degrees) //Stulen från unity
        {
            float sin = (float)Math.Sin(degrees * Deg2Rad);
            float cos = (float)Math.Cos(degrees * Deg2Rad);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }

        //public double ConvertToRadians(double angle) //Stulen från unity
        //{
        //    return (Math.PI / 180) * angle;
        //}

        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)//Stulen från unity
        {
            float sqrMagnitude = AdvancedMath.sqrMagnitude(vector);
            if (sqrMagnitude > maxLength * maxLength)
            {
                float mag = (float)Math.Sqrt(sqrMagnitude);
                //these intermediate variables force the intermediate result to be
                //of float precision. without this, the intermediate result can be of higher
                //precision, which changes behavior.
                float normalized_x = vector.X / mag;
                float normalized_y = vector.Y / mag;
                return new Vector2(normalized_x * maxLength,
                    normalized_y * maxLength);
            }
            return vector;
        }

        public static float sqrMagnitude(Vector2 temp) //Stulen från unity
        {
            return temp.X * temp.X + temp.Y * temp.Y;
        }

        public static float Magnitude(Vector2 temp) //Stulen från unity
        {
            return (float)Math.Sqrt(sqrMagnitude(temp));
        }

        public static Vector2 Right(float rotation)
        {
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            direction.Normalize();
            return AdvancedMath.Rotate(new Vector2(1, 1) * direction, 180);
        }

        public static Vector2 Right(Vector2 a, Vector2 b)
        {
            float rotation = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            direction.Normalize();
            return AdvancedMath.Rotate(new Vector2(1, 1) * direction, 180);
        }

        private static double factorialDouble(double d)
        {
            if (d == 0.0)
            {
                return 1.0;
            }

            double abs = Math.Abs(d);
            double decimalen = abs - Math.Floor(abs);
            double result = 1.0;

            for (double i = Math.Floor(abs); i > decimalen; --i)
            {
                result *= (i + decimalen);
            }
            if (d < 0.0)
            {
                result = -result;
            }

            return result;
        }

        public static bool IsDigitsOnly(string str, string operators) //Den här kollar så att det bara finns nummer eller mellanslag i passwordet. Om inte för denna så skulle spelet krasha om du skrev en bokstav.
        {
            bool containsNumber = false;
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    //if (c != ' ')
                    //{
                    bool found = false;
                    foreach (char character in operators)
                    {
                        if (c == character)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return false;
                    }
                    //}
                }
                else if (!containsNumber && c >= '0' && c <= '9')
                {
                    containsNumber = true;
                }
            }
            return containsNumber;
        }

        private const double champ = 0.123456789101112;
        private const double conway = 1.303577269034296;
        private const double phi = 1.618033988749894;

        private static double x = 1;
        private static string xString = "x";
        private static string[] _operators = { "-", "+", "/", "*", "^", "%" };

        private static string[] _singleOperators = { "!" };

        private static string[] mathConst = { "e", "pi", "tau", "champerowne", "champ", "conway", "phi", "rad2deg", "deg2rad", xString };

        private static double[] actMathConst = { Math.E, Math.PI, Math.PI * 2, champ, champ, conway, phi, Rad2Deg, Deg2Rad, x };

        private static string[] preParenthesis = { "sin^-1", "cos^-1", "tan^-1", "sin", "cos", "tan", "abs", "sqr", "log", "round", "ln" };

        private static Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2),
        (a1, a2) => a1 % a2,
        //(a1, a2) => factorialDouble(a1),
    };

        private static Func<double, double>[] preParenthesisOperation = {
        (a1) => Math.Asin(a1)*Rad2Deg,
        (a1) => Math.Acos(a1)*Rad2Deg,
        (a1) => a1 == 1 ? 45 : Math.Atan(a1)*Rad2Deg,
        (a1) => Math.Sin(a1*Deg2Rad),
        (a1) => Math.Cos(a1*Deg2Rad),
        (a1) => a1 == 45 ? 1 :Math.Tan(a1*Deg2Rad),
        (a1) => Math.Abs(a1),
        (a1) => Math.Sqrt(a1),
        (a1) => Math.Log10(a1),
        (a1) => Math.Round(a1, MidpointRounding.AwayFromZero),
        (a1) => Math.Log(a1),
        };

        private static Func<double, double>[] _singleOperations = {
        (a1) => factorialDouble(a1),
        //(a1, a2) => factorialDouble(a1),
        };

        public static double Eval(string expression)
        {
            expression = expression.Replace('.', ',');
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            MakeEverySecondOperator(tokens);

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                bool usedPreParen = false;
                if (preParenthesis.Any(a => a == token))
                {
                    if (tokens[tokenIndex + 1] != "(")
                    {
                        if (tokens[tokenIndex + 1] == "*")
                        {
                            tokens.RemoveAt(tokenIndex + 1);
                            if (tokens[tokenIndex + 1] != "(")
                            {
                                throw new ArgumentException("A pre parenthesis operation has to be followed by a parenthesis.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("A pre parenthesis operation has to be followed by a parenthesis.");
                        }
                    }
                    int index = preParenthesis.ToList().FindIndex(a => a == token);
                    tokenIndex++;
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(preParenthesisOperation[Array.IndexOf(preParenthesis, token)](Eval(subExpr)));
                    continue;
                }
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //If this is an operator
                if (Array.IndexOf(_operators, token) >= 0 || Array.IndexOf(_singleOperators, token) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        if (_singleOperators.Any(a => a == op))
                        {
                            operandStack.Push(_singleOperations[Array.IndexOf(_singleOperators, op)](arg2));
                        }
                        else
                        {
                            if (operandStack.Count > 0 || op != "-")
                            {
                                double arg1 = operandStack.Pop();
                                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                            }
                            else
                            {
                                operandStack.Push(-arg2);
                            }
                        }
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    if (IsDigitsOnly(token, ","))
                    {
                        operandStack.Push(Convert.ToDouble(token));
                    }
                    else
                    {
                        operandStack.Push(ChooseConst(token));
                    }
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                if (_singleOperators.Any(a => a == op))
                {
                    operandStack.Push(_singleOperations[Array.IndexOf(_singleOperators, op)](arg2));
                }
                else
                {
                    if (operandStack.Count > 0 || op != "-")
                    {
                        double arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    else
                    {
                        operandStack.Push(-arg2);
                    }
                }
            }
            return operandStack.Pop();
        }

        private static string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private static List<string> getTokens(string expression)
        {
            List<string> allOperators = _operators.ToList();
            allOperators.AddRange(_singleOperators);
            string operators = string.Join("", allOperators.ToArray()); //"()^*/+-%";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();
            string newExpress = expression.Replace(" ", string.Empty);
            for (int i = 0; i < newExpress.Length; i++)
            {
                char c = newExpress[i];
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                    continue;
                }
                else if (c == '(' && i > 0 && IsDigitsOnly(newExpress[i - 1].ToString(), string.Empty) || c == ')' && i + 1 < newExpress.Length && IsDigitsOnly(newExpress[i + 1].ToString(), string.Empty))
                {
                    if (c == '(' && i > 0 && IsDigitsOnly(newExpress[i - 1].ToString(), string.Empty))
                    {
                        if ((sb.Length > 0))
                        {
                            tokens.Add(sb.ToString());
                            sb.Length = 0;
                        }
                        tokens.Add("*");
                        tokens.Add(c.ToString());
                    }
                    else
                    {
                        if ((sb.Length > 0))
                        {
                            tokens.Add(sb.ToString());
                            sb.Length = 0;
                        }
                        tokens.Add(c.ToString());
                        tokens.Add("*");
                    }
                    continue;
                }
                if (c == '(' || c == ')')
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    if (i > 0 && newExpress[i - 1] == ')' && c == '(')
                    {
                        tokens.Add("*");
                    }
                    tokens.Add(c.ToString());
                }
                else if (!IsDigitsOnly(c.ToString(), string.Empty) && IsFirstLetterOfConstant(c, newExpress, i))
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    int constantsLength = ConstantLengthFrom(c, newExpress, i);
                    tokens.Add(newExpress.Substring(i, constantsLength));
                    i += constantsLength - 1;
                }
                else if (operators.IndexOf(c) < 0)
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        private static double ChooseConst(string str)
        {
            int index = mathConst.ToList().FindIndex(a => a.Contains(str.ToLower()));
            if (index >= 0)
            {
                //if (str.ToLower() == "e")
                //{
                //    return Math.E;
                //}
                //else if (str.ToLower() == "pi")
                //{
                //    return Math.PI;
                //}
                //else if (str.ToLower() == "tau")
                //{
                //    return 2 * Math.PI;
                //}
                //else if (str.ToLower().Contains("champ"))
                //{
                //    return champ;
                //}
                //else if (str.ToLower().Contains("conway"))
                //{
                //    return conway;
                //}
                for (int i = index; i < mathConst.Length; i++)
                {
                    if (str.ToLower() == mathConst[i])
                    {
                        return actMathConst[i];
                    }
                }
            }
            throw new Exception("No mathematical constant matches: " + str);
        }

        private static int ConstantLengthFrom(char c, string input, int index)
        {
            for (int i = 0; i < mathConst.Length; i++)
            {
                if (c == mathConst[i][0] && (input.Length - index > mathConst[i].Length ? input.Substring(index, mathConst[i].Length).ToLower().Contains(mathConst[i]) : input.Substring(index).ToLower().Contains(mathConst[i])))
                {
                    return mathConst[i].Length;
                }
            }
            for (int i = 0; i < preParenthesis.Length; i++)
            {
                if (c == preParenthesis[i][0] && (input.Length - index > mathConst[i].Length ? input.Substring(index, preParenthesis[i].Length).ToLower().Contains(preParenthesis[i]) : input.Substring(index).ToLower().Contains(preParenthesis[i])))
                {
                    return preParenthesis[i].Length;
                }
            }
            return 0;
        }

        private static bool IsFirstLetterOfConstant(char c, string input, int index)
        {
            for (int i = 0; i < mathConst.Length; i++)
            {
                if (c == mathConst[i][0] && (input.Length - index > mathConst[i].Length ? input.Substring(index, mathConst[i].Length).ToLower().Contains(mathConst[i]) : input.Substring(index).ToLower().Contains(mathConst[i])))
                {
                    return true;
                }
            }
            for (int i = 0; i < preParenthesis.Length; i++)
            {
                if (c == preParenthesis[i][0] && (input.Length - index > mathConst[i].Length ? input.Substring(index, preParenthesis[i].Length).ToLower().Contains(preParenthesis[i]) : input.Substring(index).ToLower().Contains(preParenthesis[i])))
                {
                    return true;
                }
            }
            return false;
        }

        private static void MakeEverySecondOperator(List<string> tokens)
        {
            List<string> allOperators = _operators.ToList();
            allOperators.AddRange(_singleOperators);
            //allOperators.Add("(");
            //allOperators.Add(")");
            string operators = string.Join("", allOperators.ToArray()); //"()^*/+-%";
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i > 0)
                {
                    if ((i < 1 || operators.IndexOf(tokens[i - 1]) < 0) && (i + 1 >= tokens.Count && tokens[i - 1] != "(" && tokens[i] == "(" ? !preParenthesis.Any(a => a == tokens[i - 1]) : true || tokens[i - 1] != "(" && (tokens[i] == "(" ? !preParenthesis.Any(a => a == tokens[i - 1]) : true) && operators.IndexOf(tokens[i]) < 0 && (i + 1 < tokens.Count ? operators.IndexOf(tokens[i + 1]) < 0 && tokens[i + 1] != ")" : true)) && tokens[i - 1] != "(" && tokens[i] != "(" && tokens[i] != ")" && (i + 1 < tokens.Count ? tokens[i + 1] != ")" && operators.IndexOf(tokens[i + 1]) < 0 : true) && operators.IndexOf(tokens[i]) < 0)
                    {
                        //if (tokens[i - 1] != "(" && tokens[i] != "(" && operators.IndexOf(tokens[i]) < 0)
                        //{
                        //    if (i + 1 < tokens.Count)
                        //    {
                        //        if (tokens[i + 1] != ")" && operators.IndexOf(tokens[i + 1]) < 0)
                        //            tokens.Insert(i, "*");
                        //    }
                        //    else
                        //    {
                        //        tokens.Insert(i, "*");
                        //    }
                        //}
                        tokens.Insert(i, "*");
                    }
                }
                //return tokens;
            }
        }

        public static string ReplaceXWithConstant(string expression, float inX, string xChar)
        {
            expression = expression.ToLower();
            //for (int i = 0; i < expression.Length; i++)
            //{
            //    char c = expression[i];
            //    if (!IsDigitsOnly(c.ToString(), string.Empty) && !IsFirstLetterOfConstant(c, expression, i) && c == xChar)
            //    {
            //        expression = expression.Remove(i, 1);
            //        expression = expression.Insert(i, x.ToString());
            //    }
            //}
            x = inX;
            xString = xChar;
            actMathConst[actMathConst.Length - 1] = x;
            mathConst[mathConst.Length - 1] = xString;

            return expression;
        }

        public static void ResetX()
        {
            mathConst[mathConst.Length - 1] = "x";
        }

        public static bool ContainsNewCons(string expression, char constant)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (!IsDigitsOnly(c.ToString(), string.Empty) && !IsFirstLetterOfConstant(c, expression, i) && c == constant)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetNearestMultiple(int value, int factor)
        {
            return (int)Math.Round(
                              (value / (double)factor),
                              MidpointRounding.AwayFromZero
                          ) * factor;
        }
    }
}