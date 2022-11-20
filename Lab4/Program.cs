using Lab4;

// 1
var gcdex = ModularMath.GcdEx(342, 612);
Console.WriteLine($"GCDEX check {342 * gcdex.x + 612 * gcdex.y == gcdex.d}");

// 2
var inverseElement = ModularMath.InverseElement(5, 18);
Console.WriteLine($"Inverse element check {inverseElement == 11}");

// 3
var phi = ModularMath.Phi(18);
Console.WriteLine($"Phi check {phi == 6}");

// 4
var inverseElementByPhi = ModularMath.InverseElementByPhi(5, 18);
Console.WriteLine($"Inverse element 2 check {inverseElementByPhi == 11}");