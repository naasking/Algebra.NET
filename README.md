# Algebra.NET

This is a simple algebra library designed to facilitate easy expression
and manipulation of algebraic functions. For instance, here's a simple
function:

    Function<Func<double, double>> a = Algebra.Function(x => 2 * x + 1);

We can compile such a function to efficient IL:

    Func<double, double> func = a.Compile("times2plus1");

Or we can apply some algebraic identities to rewrite it:

    Identity associative = Algebra.Identity(x => x + 1 == 1 + x);
    Identity mulEqAdd = Algebra.Identity(x => 2 * x == x + x);
	Console.WriteLine(a);
	Console.WriteLine(a.Rewrite(1, associative, mulEqAdd));

	// Prints:
	// ((2 * x) + 1)
	// (1 + (x + x))

Rewrites can sometimes loop forever, so the Rewrite method takes a
number indicating the maximum number of iterations to perform.

All the usual arithmetic operations are available, including an
extension method for exponentiation:

    var f = Algebra.Function(x => x.Pow(3));
	Console.WriteLine(x);

	// Prints:
	// (x ^ (3))

# Design

It's a nice, functional example of a simple term rewriting system. Term
rewriting is usually pretty awkward in an object-oriented language,
and I banged my head against the keyboard to figure out a nice
way to do it, until I hit on just doing a simple pattern match.

So I reused the term language and added an equality operator to
generate an identity that conceptually maps one term to another.
I then pattern match on the left hand side, and generate a set of
substitutions to transform the matching term into the right hand side
of the identity.

It was ultimately quite simple, consisting of 3 methods on Term:

	static Term Rewrite(Term current, int rounds, Identity[] equalities, Term[] bindings)
    bool TryMatch(Term e, Term[] bindings)
    Term Subsitute(Term[] bindings)

Rewrite tries to recursively match the Identity's left hand side with
the current term using TryMatch. On success, the 'bindings' array
will have been populated by TryMatch with the substitutions to perform,
so it substitutes the bindings into the identity's right hand side to
generate the new term.

There are only 3 term types: constants, variables and binary
operations. Negation is handled as a binary operation "0 - x" for
simplicity.

So if you want to understand expression compilation to CIL,
pattern matching, or term rewriting, this is pretty much as simple
as it gets.

Algebra.NET doesn't perform any term simplification at this point,
only term rewriting. Some rewrites may of course be simplifications,
but a term like "0 - 3" will not be simplified to "-3".

# Future Work

 * expression simplification
 * automatic and symbolic differentiation
 * nested function calls?

# License

LGPL version 2.1