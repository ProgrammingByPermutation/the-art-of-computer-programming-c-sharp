using System;
using System.Collections.Generic;
using System.Linq;

namespace Fundamental_Algorithms.Basic_Concepts
{
    /// <summary>
    /// A class of algorithms relating to permutations.
    /// </summary>
    public class Permutation
    {
        /// <summary>
        /// The unique notation of the cycle form.
        /// </summary>
        public string CanonicalForm { get; private set; }

        /// <summary>
        /// The unique notation of the cycle form with parenthesis.
        /// </summary>
        public string CanonicalFormNoParen { get; private set; }

        /// <summary>
        /// The permutation in cycle form. Ex: (abc)(de)(fg)
        /// </summary>
        public string CycleForm { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cycleForm">The permutation in cycle form. Ex: (abc)(de)(fg)</param>
        public Permutation(string cycleForm) {
            if (string.IsNullOrWhiteSpace(cycleForm)) {
                throw new NullReferenceException("Error permutation's cycle form cannot be null or empty.");
            }

            if (!cycleForm.StartsWith("(") || !cycleForm.EndsWith(")")) {
                throw new FormatException("Error permutation's cycle form must begin and end with parenthesis.");
            }

            CycleForm = cycleForm;
            CanonicalForm = ToCanonicalForm(cycleForm);

            // Ensure conversion was successful
            if (CanonicalForm != null) {
                CanonicalFormNoParen = CanonicalForm.Replace("(", "").Replace(")", "");
            }
        }

        /// <summary>
        /// Multiplies two permutations.
        /// </summary>
        /// <param name="left">The left hand permutation.</param>
        /// <param name="right">The right hand permutation.</param>
        /// <returns>The product of the permutations.</returns>
        public static Permutation operator *(Permutation left, Permutation right) {
            if (null == left || string.IsNullOrWhiteSpace(left.CycleForm) ||
                null == right || string.IsNullOrWhiteSpace(right.CycleForm)) {
                return null;
            }

            return new Permutation(Multiply(left.CycleForm.Trim() + right.CycleForm.Trim()));
        }

        /// <summary>
        /// Converts permutation from its cycle form representation to a unique
        /// canonical form representation.
        /// </summary>
        /// <remarks>
        /// Section 1.3.3 Applications to Permutations
        /// </remarks>
        /// <param name="cycleForm">The cycle form representation of the permutation.</param>
        /// <returns>The canonical representation of the permutation, null if unsuccessful.</returns>
        public static string ToCanonicalForm(string cycleForm) {
            if (null == cycleForm) {
                return null;
            }

            // Get rid of the first ( so we can split on it. 
            // Remove all of the ), they'll just get in the way.
            string canonicalForm = cycleForm.TrimStart('(').Replace(")", "");

            // Split each cycle
            string[] cycles = canonicalForm.Split('(');

            // Put it into a form we can use
            List<char[]> cycleList = new List<char[]>(cycles.Length);
            cycleList.AddRange(cycles.Select(t => (null != t) ? t.ToArray() : null));

            // Step A. Write all the singletons explicitly. (We do this anyway,
            // so there is no additional processing)

            // Step B. Within each cycle, put the smallest number first.
            foreach (var currCycle in cycleList) {
                if (null == currCycle) {
                    // Something went wrong
                    return null;
                }

                // Find the smallest number
                int indexOfSmallest = 0;
                char smallest = char.MaxValue;
                for (int i = 0; i < currCycle.Length; i++) {
                    char currChar = currCycle[i];
                    if (currChar < smallest) {
                        smallest = currChar;
                        indexOfSmallest = i;
                    }
                }

                // Left shift everything
                if (indexOfSmallest > 0) {
                    int offset = indexOfSmallest % currCycle.Length;

                    if (offset < 0)
                        offset = currCycle.Length + offset;

                    char[] temp = new char[offset];
                    Array.Copy(currCycle, temp, offset);
                    Array.Copy(currCycle, offset, currCycle, 0, currCycle.Length - offset);
                    Array.Copy(temp, 0, currCycle, currCycle.Length - offset, temp.Length);
                }
            }

            // Step C. Order the cycles in decreasing order of the first number 
            // in the cycle.
            cycleList = new List<char[]>(cycleList.OrderByDescending(c => (null != c && c.Length > 0) ? c[0] : -1));

            // Join them together again
            return "(" + string.Join(")(", cycleList.Select(c => new string(c))) + ")";
        }

        /// <summary>
        /// Multiplies a permutation in cycle form.
        /// </summary>
        /// <remarks>
        /// Section 1.3.3 Applications to Permutations: Algorithm A
        /// </remarks>
        /// <param name="cycleForm">A series of permutations to multiply in cycle form.
        /// Expected form (abc)(de)(fg).</param>
        /// <returns>The product of a series of permutations in cycle form.</returns>
        public static string Multiply(string cycleForm) {
            // Sanity check
            if (string.IsNullOrWhiteSpace(cycleForm)) {
                return null;
            }

            // Convert to a format we can manipulate
            char[] cycleArray = cycleForm.ToCharArray();

            // A1. Tag all left parenthesis, and replace each right parenthesis
            // by a tagged copy of the input symbol that follows its matching
            // left parenthesis 
            bool foundOpenParen = false;
            char afterOpen = default(char);
            bool[] taggedEntries = new bool[cycleArray.Length];
            for (int i = 0; i < cycleArray.Length; i++) {
                char currChar = cycleArray[i];
                if (foundOpenParen) {
                    afterOpen = currChar;
                }

                foundOpenParen = currChar == '(';
                taggedEntries[i] = foundOpenParen;
                if (currChar == ')') {
                    if (afterOpen == default(char)) {
                        throw new FormatException("Cycle is incorrectly formatted! Found " +
                                                  "close parenthesis with no matching open.");
                    }

                    cycleArray[i] = afterOpen;
                    taggedEntries[i] = true;
                }
            }

            // Declare the variables we will use A2 -> A6
            List<char> output = new List<char>();
            char START = default(char), CURRENT = default(char);
            bool doA2 = true, doA3 = true;

            // A2. (If all elements are tagged, the algorithm terminates.) 
            while (!taggedEntries.Aggregate(true, (b, b1) => b && b1)) {
                bool scanningFormula = true;
                while (scanningFormula) {
                    for (int i = 0; i < cycleArray.Length; i++) {
                        // A2. Searching from left to right, find the first untagged element of the input.
                        // Set START equal to it; output a left parenthesis; output the element; and tag it.
                        if (doA2) {
                            if (taggedEntries[i]) {
                                continue;
                            }

                            START = cycleArray[i];
                            output.AddRange(new[] {'(', START});
                            taggedEntries[i] = true;
                            doA2 = false;
                            continue;
                        }

                        // A3. Set CURRENT equal to the next element of the formula.
                        if (doA3) {
                            CURRENT = cycleArray[i];
                            doA3 = false;
                            continue;
                        }

                        // A4. Proceed to the right until either reaching the end of the formula, or finding an 
                        // element equal to CURRENT; in the latter case, tag it and go back to step A3.
                        if (cycleArray[i] == CURRENT) {
                            taggedEntries[i] = true;
                            doA3 = true;
                        }
                    }

                    // A5. If CURRENT != START, output CURRENT and go back to step A4 starting again
                    // at the left of the formula (thereby continuing the development of the cycle
                    // in the output).
                    if (CURRENT != START) {
                        output.Add(CURRENT);
                        doA2 = false;
                        doA3 = false;
                    }
                    else {
                        output.Add(')');
                        doA2 = true;
                        doA3 = true;
                        scanningFormula = false;
                    }
                }
            }

            return new string(output.ToArray());
        }

        /// <summary>
        /// Returns the inverse of a permutation.
        /// </summary>
        /// <remarks>
        /// Section 1.3.3 Applications to Permutations: Algorithm I
        /// </remarks>
        /// <param name="intArray">The array to invert.</param>
        /// <returns>The inverted array, null if unsuccessful.</returns>
        public static int[] InverseIntArrayI(int[] intArray) {
            // Sanity check
            if (null == intArray || intArray.Length == 0) {
                return null;
            }

            // Algorithm uses a 1 based index
            int[] X = new int[intArray.Length + 1];
            Array.Copy(intArray, 0, X, 1, intArray.Length);

            // I1. [Initialize] Set m <- n, j <- -1
            int j = -1;

            // I6. Loop on m
            for (int m = X.Length - 1; m >= 0; m--) {
                // I2. [Next Element] Set i <- X[m]. If i < 0, go to step I5 (the element
                // has already been processed).
                int i = X[m];
                bool doI5 = i < 0;

                if (!doI5) {
                    // I3. Set X[m] <- j, j <- -m, m <- i, i <- X[m]
                    bool doI3 = true;
                    while (doI3) {
                        doI3 = false;
                        X[m] = j;
                        j = -m;
                        m = i;
                        i = X[m];

                        // I4. If i > 0, go back to I3 (the cycle has not ended);
                        // otherwise set i <- j
                        if (i > 0) {
                            doI3 = true;
                        }
                        else {
                            i = j;
                        }
                    }
                }

                // I5. Set X[m] <- -i
                X[m] = -i;
            }

            // Convert back to zero based index
            Array.Copy(X, 1, intArray, 0, intArray.Length);
            return intArray;
        }

        /// <summary>
        /// Returns the inverse of a permutation.
        /// </summary>
        /// <remarks>
        /// Section 1.3.3 Applications to Permutations: Algorithm J
        /// </remarks>
        /// <param name="intArray">The array to invert.</param>
        /// <returns>The inverted array, null if unsuccessful.</returns>
        public static int[] InverseIntArrayJ(int[] intArray) {
            // Sanity check
            if (null == intArray || intArray.Length == 0) {
                return null;
            }

            // Algorithm uses a 1 based index
            int[] X = new int[intArray.Length + 1];
            Array.Copy(intArray, 0, X, 1, intArray.Length);

            // J1. Set X[k] <- -X[k], for 1 <= k <= n. Also set m <- n.
            for (int z = 0; z < X.Length; z++) {
                X[z] = -X[z];
            }

            // J5. Decrease m by 1 if m > 0 go back to J2. Otherwise the algorithm terminates.
            for (int m = X.Length - 1; m >= 0; m--) {
                // J2. Initialize j
                int j = m;

                // J3. Set i <- X[j]. If i > 0, set j <- i and repeat this step.
                int i;
                while (true) {
                    i = X[j];
                    if (i > 0) {
                        j = i;
                    }
                    else {
                        break;
                    }
                }

                // J4. Set X[j] <- X[-i], X[-i] <- m
                X[j] = X[-i];
                X[-i] = m;
            }

            // Convert back to zero based index
            Array.Copy(X, 1, intArray, 0, intArray.Length);
            return intArray;
        }
    }
}