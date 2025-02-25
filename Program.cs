var solution = new Solution();

Console.WriteLine(solution.CanReachCorner(
    22742157, 210809967, new int[][] {
        new int[] { 22741186, 210810964, 200 },
        new int[] { 22741869, 210809432, 165 },
        new int[] { 22742256, 210810275, 182 },
        new int[] { 22742089, 210809693, 129 },
        new int[] { 22741912, 210810128, 196 },
        new int[] { 22741658, 210809205, 144 },
        new int[] { 22741648, 210809094, 118 },
        new int[] { 22741920, 210809808, 128 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    500000000, 500000000, new int[][] {
        new int[] { 499980000, 699999999, 200000000 },
        new int[] { 500020000, 300000001, 200000000 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    8, 6, new int[][] {
        new int[] { 6, 1, 1 },
        new int[] { 7, 2, 1 },
        new int[] { 6, 3, 1 },
        new int[] { 3, 4, 2 },
        new int[] { 6, 2, 2 },
        new int[] { 4, 4, 2 },
        new int[] { 7, 3, 1 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    20, 100, new int[][] {
        new int[] { 1, 102, 18 },
        new int[] { 50, 60, 48 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    3, 4, new int[][] {
        new int[] { 2, 1, 1 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    3, 3, new int[][] {
        new int[] { 1, 1, 2 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    3, 3, new int[][] {
        new int[] { 2, 1, 1 }, new int[] { 1, 2, 1 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    4, 4, new int[][] {
        new int[] { 5, 5, 1 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    5, 9, new int[][] {
        new int[] { 4, 7, 1 },
        new int[] { 2, 1, 1 },
        new int[] { 4, 7, 1 },
        new int[] { 3, 7, 1 },
        new int[] { 4, 1, 1 },
        new int[] { 4, 7, 1 },
        new int[] { 1, 5, 1 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    6, 13, new int[][] {
        new int[] { 1, 5, 1 },
        new int[] { 1, 5, 1 },
        new int[] { 5, 7, 1 },
        new int[] { 3, 7, 2 },
        new int[] { 5, 5, 1 },
        new int[] { 2, 10, 1 },
        new int[] { 2, 1, 1 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    15, 15, new int[][] {
        new int[] { 1, 99, 85 },
        new int[] { 99, 1, 85 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    5, 8, new int[][] {
        new int[] { 4, 7, 1 } })
    == false ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    13, 13, new int[][] {
        new int[] { 10, 5, 3 },
        new int[] { 1, 2, 1 },
        new int[] { 3, 8, 1 },
        new int[] { 2, 12, 1 },
        new int[] { 10, 1, 1 },
        new int[] { 7, 4, 1 },
        new int[] { 4, 5, 3 } })
    == true ? "CORRECT" : "INCORRECT");

Console.WriteLine(solution.CanReachCorner(
    283239, 179963,
    new int[][] {
        new int[] { 248866, 18768, 15302 },
        new int[] { 118187, 107493, 44573 },
        new int[] { 108498, 120943, 43664 },
        new int[] { 153333, 112887, 34787 },
        new int[] { 177345, 57622, 13897 },
        new int[] { 110613, 49502, 49502 },
        new int[] { 55969, 48432, 13190 },
        new int[] { 77476, 58814, 35515 },
        new int[] { 143118, 79684, 31 } })
    == true ? "CORRECT" : "INCORRECT");
