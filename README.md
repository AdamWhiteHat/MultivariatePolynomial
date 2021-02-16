# MultivariatePolynomial


A **symbolic**, multivariate integer polynomial arithmetic library.


An example of a multivariate polynomial would be:

    16*x*y^2 +  12*x*y -  4*y - z


* Supports **symbolic** multivariate polynomial arithmetic, including:
   * Addition
   * Subtraction
   * Multiplication
   * Division
   * Exponentiation
   * Derivatives
   * Functional composition
   * Polynomial evaluation by assigning values to the indeterminates
   * Numeric values are of type BigInteger, so it support polynomials that evaluate to arbitrarily large numbers
   * While all coefficients must be integers, it does support evaluating the polynomial with real and complex indeterminates, returning a real or complex result

