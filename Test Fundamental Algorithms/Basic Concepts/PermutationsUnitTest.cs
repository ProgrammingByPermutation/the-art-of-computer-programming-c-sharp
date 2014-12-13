using Fundamental_Algorithms.Basic_Concepts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_Fundamental_Algorithms.Basic_Concepts
{
    [TestClass]
    public class PermutationsUnitTest
    {
        [TestMethod]
        public void MultiplyTestMethod() {
            // (acf)(bd)(abd)(ef)=(acefb)
            Assert.AreEqual(Permutation.Multiply("(acfg)(bcd)(aed)(fade)(bgfae)"), "(adg)(ceb)(f)");

            Permutation first = new Permutation("(acfg)");
            Permutation second = new Permutation("(bcd)");
            Permutation third = new Permutation("(aed)");
            Permutation fourth = new Permutation("(fade)");
            Permutation fifth = new Permutation("(bgfae)");
            Permutation result = first * second * third * fourth * fifth;

            Assert.IsNotNull(result);
            Assert.AreEqual("(adg)(bce)(f)", result.CycleForm); // ceb offset by 1 is ok

            // (acf)(bd)(abd)(ef)=(acefb)
            Assert.AreEqual(Permutation.Multiply("(acf)(bd)(abd)(ef)"), "(acefb)(d)");

            first = new Permutation("(acf)");
            second = new Permutation("(bd)");
            third = new Permutation("(abd)");
            fourth = new Permutation("(ef)");
            result = first * second * third * fourth;

            Assert.IsNotNull(result);
            Assert.AreEqual("(acefb)(d)", result.CycleForm);
        }

        [TestMethod]
        public void InvertTestMethod() {
            int[] realPermutation = {6, 2, 1, 5, 4, 3};
            int[] realInverse = {3, 2, 6, 5, 4, 1};

            // -(621543) = (326541)
            int[] inverse = Permutation.InverseIntArrayI(realPermutation);
            CollectionAssert.AreEqual(realInverse, inverse);

            // Taking the inverse of the inverse will give us the original permutation again
            inverse = Permutation.InverseIntArrayI(inverse);
            CollectionAssert.AreEqual(realPermutation, inverse);

            // -(621543) = (326541)
            inverse = Permutation.InverseIntArrayJ(realPermutation);
            CollectionAssert.AreEqual(realInverse, inverse);

            // Taking the inverse of the inverse will give us the original permutation again
            inverse = Permutation.InverseIntArrayJ(inverse);
            CollectionAssert.AreEqual(realPermutation, inverse);
        }

        [TestMethod]
        public void CanonicalFormTestMethod() {
            Permutation perm = new Permutation("(316)(54)(2)");
            Assert.AreEqual("452163", perm.CanonicalFormNoParen);
            Assert.AreEqual("(45)(2)(163)", perm.CanonicalForm);
        }
    }
}