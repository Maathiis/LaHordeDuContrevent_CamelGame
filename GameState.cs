using System;

public class GameState
{
        public int jour;
        public int zone;
        public int distanceParcourue;
        public int distanceObjectif;
        public int energie;
        public int maxEnergie;
        public int nourriture;
        public int maxNourriture;
        public int eau;
        public int maxEau;
        public int tempeteDistance;
        public int santeMentale;
        public int maxSanteMentale;
        public bool isRunning = false;
        public bool windStormOccured = false;
        public bool gameOver = false;

    public GameState()
        {
            jour = 1;
            zone = 1;
            distanceParcourue = 0;
            distanceObjectif = 500000;
            energie = 8;
            maxEnergie = 8;
            nourriture = 10;
            maxNourriture = 10;
            eau = 10;
            maxEau = 10;
            tempeteDistance = 0;
            santeMentale = 5;
            maxSanteMentale = 5;
        }
}
