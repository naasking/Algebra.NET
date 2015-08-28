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
        public static Function<Func<double, double>> Function(Func<Term, Function<Func<double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            return body(x0).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            return body(x0);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double>> Function(Func<Term, Term, Function<Func<double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            return body(x0, x1).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            return body(x0, x1);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double>> Function(Func<Term, Term, Term, Function<Func<double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            return body(x0, x1, x2).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            return body(x0, x1, x2);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Function<Func<double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            return body(x0, x1, x2, x3).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            return body(x0, x1, x2, x3);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            return body(x0, x1, x2, x3, x4).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            return body(x0, x1, x2, x3, x4);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            return body(x0, x1, x2, x3, x4, x5).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            return body(x0, x1, x2, x3, x4, x5);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            return body(x0, x1, x2, x3, x4, x5, x6).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            return body(x0, x1, x2, x3, x4, x5, x6);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            return body(x0, x1, x2, x3, x4, x5, x6, x7).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            return body(x0, x1, x2, x3, x4, x5, x6, x7);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            var x14 = new Term(14);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            var x14 = new Term(14);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double>> Function(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Function<Func<double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double,  double>>> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            var x14 = new Term(14);
            var x15 = new Term(15);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15).With(p);
        }
		
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Term, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x0 = new Term(0);
            var x1 = new Term(1);
            var x2 = new Term(2);
            var x3 = new Term(3);
            var x4 = new Term(4);
            var x5 = new Term(5);
            var x6 = new Term(6);
            var x7 = new Term(7);
            var x8 = new Term(8);
            var x9 = new Term(9);
            var x10 = new Term(10);
            var x11 = new Term(11);
            var x12 = new Term(12);
            var x13 = new Term(13);
            var x14 = new Term(14);
            var x15 = new Term(15);
            return body(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15);
        }

	}
}