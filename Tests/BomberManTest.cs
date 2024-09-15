using Model.Board;
namespace Tests
{
    [TestClass]
    public class BomberManTest
    {
        GameBoard board = new(11, 11);
        [TestMethod]
        public void InitializationTest()
        {
            Assert.AreEqual(11, board.Width);                                   //11 széles lett a pálya
            Assert.AreEqual(11, board.Height);                                  //11 magas lett a a pálya
            Assert.AreEqual(4, board.Monsters.Count);                           //4 szörny spawnolt
            Assert.AreEqual(2, board.Players.Length);                           //2 játékos spawnolt
            Assert.AreEqual((1, 5), (board.Players[0].X, board.Players[0].Y));  //Jók a koordináták
            Assert.AreEqual((9, 5), (board.Players[1].X, board.Players[1].Y));  //Jók a koordináták
        }
        [TestMethod]
        public void PlayerTest()
        {
            Assert.IsFalse(board.Players[0].Move(0));                           //Fenti játékos nem tud már felfelé mozogni
            Assert.IsTrue(board.Players[1].Move(0));                            //Lenti játékos tud felfelé menni
            Assert.AreEqual(board.Players[0].BombCount, 1);                     //1 bomba alapbol
            board.Players[0].AddBomb();                                         //Adunk 1 bombat neki
            Assert.AreEqual(board.Players[0].BombCount, 2);                     //2 lett :O
            Assert.IsTrue(board.Players[0].Alive);                              //1. játékos él
            board.Players[0].Kill();                                            //Megöljük
            Assert.IsFalse(board.Players[0].Alive);                             //Tényleg halott :O
        }
        [TestMethod]
        public void BombTest()
        {
            board.Players[1].PlaceBomb();                                       //Lerakjuk a bombát
            Assert.AreEqual(board.Bombs.Count, 1);                              //Tényleg lerakódott woooow
        }
    }
}