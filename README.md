# Algebra.NET

This is a simple algebra library designed to facility easy expression
and manipulation of algebraic functions. For instance, here's a simple
function that increments a variable by 1:

    Function<Func<double, double>> a = Algebra.Function(x => 2 * x + 1);

Now we can compile it to efficient IL:

    Func<double, double> func = a.Compile("times2plus1");

Or we can apply some algebraic identities to rewrite it:

    Identity associative = Algebra.Identity(x => x + 1 == 1 + x);
    Identity mulEqAdd = Algebra.Identity(x => 2 * x == x + x);
	Console.WriteLine(a);
	Console.WriteLine(a.Rewrite(1, associative, mulEqAdd));

	// Prints:
	// ((2 * x) + 1)
	// (1 + (x + x))

# Future Work

 * expression simplification
 * automatic and symbolic differentiation

# License

LGPL version 2.1