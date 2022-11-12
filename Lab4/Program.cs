using Lab4;

// 1
var gcdex = Algo.GcdEx(342, 612);
Console.WriteLine($"GCDEX check {342 * gcdex.x + 612 * gcdex.y == gcdex.d}");

// 2
var inverseElement = Algo.InverseElement(5, 18);
Console.WriteLine($"Inverse element check {inverseElement == 11}");

// 3
var phi = Algo.Phi(18);
Console.WriteLine($"Phi check {phi == 6}");

// 4
var inverseElementByPhi = Algo.InverseElementByPhi(5, 18);
Console.WriteLine($"Inverse element 2 check {inverseElementByPhi == 11}");