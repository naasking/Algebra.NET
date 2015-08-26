using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newton;

namespace TestAlgebra
{
    class Program
    {
        static void Plus1()
        {
            var a = Algebra.Function(x => x + 1);
            var f = a.Compile("plus1");
            Debug.Assert(f(0) == 1);
            Debug.Assert(f(1) == 2);
            Debug.Assert(f(-1) == 0);
            Debug.Assert(a.Rewrite() == a);
        }

        static void PlusX()
        {
            var a = Algebra.Function((x, y) => x + y);
            var f = a.Compile("plusX");
            Debug.Assert(f(0, 0) == 0);
            Debug.Assert(f(0, 1) == 1);
            Debug.Assert(f(1, 0) == 1);
            Debug.Assert(f(-1, 0) == -1);
            Debug.Assert(a.Rewrite() == a);
        }

        static void MulX()
        {
            var a = Algebra.Function((x, y) => 1 + x * y);
            var f = a.Compile("mulX");
            Debug.Assert(f(0, 0) == 1);
            Debug.Assert(f(0, 1) == 1);
            Debug.Assert(f(1, 0) == 1);
            Debug.Assert(f(-1, 1) == 0);
            Debug.Assert(a.Rewrite() == a);
        }

        static void PowX()
        {
            var a = Algebra.Function((x, y) => 1 + x.Pow(y));
            var f = a.Compile("mulX");
            Debug.Assert(f(0, 0) == 2);
            Debug.Assert(f(0, 1) == 1);
            Debug.Assert(f(1, 0) == 2);
            Debug.Assert(f(2, 10) == 1 + Math.Pow(2, 10));
            Debug.Assert(f(-1, 1) == 0);
            Debug.Assert(a.Rewrite() == a);
        }

        static void Associative()
        {
            var eq = Algebra.Identity((x, y) => x + y == y + x);
            var f = Algebra.Function(x => 1 + x);
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.Rewrite(1, eq).ToString() == "(x + 1)");
        }

        static void Distributive()
        {
            var eq = Algebra.Equality((x, y, z) => z * (x + y) == z * y + z * x);
            var f = Algebra.Function(x => 3 * (x + 1));
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.Rewrite(1, eq).ToString() == "((3 * 1) + (3 * x))");
        }

        static void Factor()
        {
            var eq = Algebra.Equality((x, y, z) => z * y + z * x == z * (x + y));
            var f = Algebra.Function((x, y) => 3 * x + 3 * y);
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.Rewrite(1, eq).ToString() == "(3 * (y + x))");
        }

        static void Negate()
        {
            var eq = Algebra.Identity((x, y) => -(x + y) == -x - y);
            var f = Algebra.Function((x, y) => -(3 + x + y));
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.Rewrite(2, eq).ToString() == "(((0 - 3) - x) - y)");
        }

        static void Pow()
        {
            var eq = Algebra.Identity((x, y) => x * x == x.Pow(2));
            var f = Algebra.Function((x, y) => (x + 1) * (x + 1));
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.Rewrite(2, eq).ToString() == "((x + 1) ^ (2))");
        }

        static void ReadmeSample()
        {
            var f = Algebra.Function(x => 2 * x + 1);
            Identity associative = Algebra.Identity(x => x + 1 == 1 + x);
            Identity mulEqAdd = Algebra.Identity(x => 2 * x == x + x);
            Debug.Assert(f.Rewrite() == f);
            Debug.Assert(f.ToString() == "((2 * x) + 1)");
            Debug.Assert(f.Rewrite(1, associative, mulEqAdd).ToString() == "(1 + (x + x))");
        }

        static void Main(string[] args)
        {
            Plus1();
            PlusX();
            MulX();
            PowX();
            Associative();
            Distributive();
            Factor();
            Negate();
            Pow();
            ReadmeSample();

            Console.WriteLine("Tests complete...");
            Console.ReadLine();
        }
    }
}
