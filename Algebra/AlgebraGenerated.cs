using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace AlgebraDotNet
{
	public static partial class Algebra
	{
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double,  double>> Function(Func<Variable, Function<Func<double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            return body(x0);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            return body(x0);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double,  double>> Function(Func<Variable,Variable, Function<Func<double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            return body(x0, x1);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            return body(x0, x1);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double,  double>> Function(Func<Variable,Variable,Variable, Function<Func<double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            return body(x0, x1, x2);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            return body(x0, x1, x2);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable, Function<Func<double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            return body(x0, x1, x2, x3);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            return body(x0, x1, x2, x3);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            return body(x0, x1, x2, x3, x4);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            return body(x0, x1, x2, x3, x4);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            return body(x0, x1, x2, x3, x4, x5);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            return body(x0, x1, x2, x3, x4, x5);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            return body(x0, x1, x2, x3, x4, x5, x6);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            return body(x0, x1, x2, x3, x4, x5, x6);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            return body(x0, x1, x2, x3, x4, x5, x6, x7);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            return body(x0, x1, x2, x3, x4, x5, x6, x7);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            var x14 = new Variable(p[14].Name, 14);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            var x14 = new Variable(p[14].Name, 14);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>> Function(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            var x14 = new Variable(p[14].Name, 14);
            var x15 = new Variable(p[15].Name, 15);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable,Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Variable(p[0].Name, 0);
            var x1 = new Variable(p[1].Name, 1);
            var x2 = new Variable(p[2].Name, 2);
            var x3 = new Variable(p[3].Name, 3);
            var x4 = new Variable(p[4].Name, 4);
            var x5 = new Variable(p[5].Name, 5);
            var x6 = new Variable(p[6].Name, 6);
            var x7 = new Variable(p[7].Name, 7);
            var x8 = new Variable(p[8].Name, 8);
            var x9 = new Variable(p[9].Name, 9);
            var x10 = new Variable(p[10].Name, 10);
            var x11 = new Variable(p[11].Name, 11);
            var x12 = new Variable(p[12].Name, 12);
            var x13 = new Variable(p[13].Name, 13);
            var x14 = new Variable(p[14].Name, 14);
            var x15 = new Variable(p[15].Name, 15);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15);
        }

	}
}