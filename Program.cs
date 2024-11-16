using System;
using System.Collections.Generic;

namespace CamelGame
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Initialisation des variables de départ
            GameState gameState = InitializeGame();
            Console.WriteLine(text.Rules);
            Console.WriteLine(text.Start);
            Console.ReadLine();
            // Boucle principale du jeu
            while (!gameState.gameOver && gameState.jour <= 33 && gameState.distanceParcourue < gameState.distanceObjectif && gameState.energie > 0)
            {
                DisplayGamePhase(gameState);
                Vent ventActuel = SelectWindForZone(gameState.zone);
                if (gameState.jour == 5)
                {
                    AnnounceStorm();
                }

                if (gameState.jour == 6)
                {
                    gameState.tempeteDistance += 15000;
                }
                DailyEvent(gameState);
                if (gameState.windStormOccured)
                {
                    gameState.windStormOccured = false;
                    continue;
                }
                DisplayWindInfo(ventActuel);
                ApplyWindEffects(ventActuel, gameState);
                ExecuteTwoActions(gameState, ventActuel);
                UpdateGameState(ref gameState);
                CheckGameOverConditions(gameState);
            }

            DisplayGameResult(gameState);
        }

        // Initialisation du jeu
        static GameState InitializeGame()
        {
            return new GameState();
        }
        // Afficher l'état de la phase de jeu
        static void DisplayGamePhase(GameState gameState)
        {
            Console.WriteLine($"\n*** Zone {gameState.zone} - Jour {gameState.jour} ***");
        }
        // Choisir un vent en fonction de la zone
        static Vent SelectWindForZone(int zone)
        {
            Random rand = new Random();
            int chance = rand.Next(0, 101); // Valeur entre 0 et 100

            // Définir les probabilités pour chaque zone
            if (zone == 1)
            {
                if (chance <= 60) return Vent.Zefirine; // 60%
                if (chance <= 90) return Vent.Slamino; // 30%
                return Vent.Steche; // 10%
            }
            else if (zone == 2)
            {
                if (chance <= 40) return Vent.Zefirine; // 40%
                if (chance <= 70) return Vent.Slamino; // 30%
                if (chance <= 90) return Vent.Steche; // 20%
                return Vent.Choon; // 10%
            }
            else if (zone == 3)
            {
                if (chance <= 20) return Vent.Zefirine; // 20%
                if (chance <= 60) return Vent.Slamino; // 40%
                if (chance <= 80) return Vent.Steche; // 20%
                return Vent.Choon; // 20%
            }
            else if (zone == 4)
            {
                if (chance <= 5) return Vent.Zefirine; // 0 à 5 = 5
                if (chance <= 30) return Vent.Slamino; // 5 à 30 = 25
                if (chance <= 60) return Vent.Steche; // 30 à 60 = 30
                return Vent.Choon; // 60 à 100 = 40
            }
            else
            {
                if (chance <= 5) return Vent.Zefirine; // 0 à 5 = 5
                if (chance <= 20) return Vent.Choon; // 5 à 65 = 60
                if (chance <= 90) return Vent.Crivetz; // 65 à 90 = 25
                return Vent.Furvent; // 90 à 100 = 10
            }
        }

        static void ApplyWindEffects(Vent vent, GameState gameState)
        {
            switch (vent)
            {
                case Vent.Zefirine:
                    // Pas d'effet, vent neutre
                    break;
                case Vent.Slamino:
                    Console.WriteLine(text.Slamino);
                    gameState.energie--; // Réduit l'énergie de 1 et empêche la course pour 1 action
                    break;
                case Vent.Steche:
                    Console.WriteLine(text.Steche);
                    gameState.energie--; // Réduit l'énergie de 1 et empêche la course pour les 2 actions
                    break;
                case Vent.Choon:
                    Console.WriteLine(text.Choon);
                    gameState.energie--; // Réduit l'énergie de 1
                    break;
                case Vent.Crivetz:
                    Console.WriteLine(text.Crivetz);
                    gameState.energie -= 2; // Réduit l'énergie de 2
                    break;
                case Vent.Furvent:
                    Console.WriteLine(text.Furvent);
                    gameState.energie -= 3; // Réduit l'énergie de 3
                    gameState.nourriture--; // Réduit aussi la nourriture de 1
                    break;
            }
        }
        // Afficher les informations sur le vent
        static void DisplayWindInfo(Vent vent)
        {
            // Utilise la méthode de la classe text pour obtenir la description du vent
            string description = text.GetWindDescription(vent);
            Console.WriteLine($"* Vent : {description} *");
        }
        // Annonce de la tempête
        static void AnnounceStorm()
        {
            Console.WriteLine(text.AnnounceStorm);
        }
        // Afficher l'état des ressources
        static void DisplayResources(GameState gameState)
        {
            Console.WriteLine($"⚡ : {gameState.energie}/{gameState.maxEnergie}, 🍖 : {gameState.nourriture}/{gameState.maxNourriture}, 💧 : {gameState.eau}/{gameState.maxEau}, \U0001f9e0 : {gameState.santeMentale}/{gameState.maxSanteMentale}");
        }
        // Exécuter deux actions du joueur
        static void ExecuteTwoActions(GameState gameState, Vent ventActuel)
        {
            bool hasRunThisTurn = false;
            for (int actionCount = 0; actionCount < 2; actionCount++)
            {
                DisplayResources(gameState);
                int choix = ChooseAction(gameState, ventActuel, hasRunThisTurn);
                UpdateGameStateAfterAction(choix, gameState, ventActuel, hasRunThisTurn);
                if (choix == 2) 
                {
                    hasRunThisTurn = true;
                }
            }
        }
        // Choisir une action
        static int ChooseAction(GameState gameState, Vent ventActuel, bool hasRunThisTurn)
        {
            Console.WriteLine("\nQue voulez-vous faire ?");
            if (CanWalk(gameState)) Console.WriteLine("1 - Marcher (−1 ⚡, −1 🍖, −1 💧)");
            if (CanRun(gameState, ventActuel, hasRunThisTurn)) Console.WriteLine("2 - Courir (−2 ⚡, −2 🍖, −2 💧)");
            if (CanRest(gameState)) Console.WriteLine("3 - Se reposer (+3 ⚡, +1 🧠)");
            if (CanSearchResources(gameState)) Console.WriteLine("4 - Chercher des ressources (−1 ⚡, +1 🍖 ou/et 💧)");

            return int.Parse(Console.ReadLine());
        }
        // Vérifier si le joueur peut marcher
        static bool CanWalk(GameState gameState) => gameState.eau > 0 && gameState.nourriture > 0 && gameState.energie > 0;
        // Vérifier si le joueur peut courir
        static bool CanRun(GameState gameState, Vent ventActuel, bool hasRunThisTurn) => ventActuel != Vent.Steche && (ventActuel != Vent.Slamino || !hasRunThisTurn) && gameState.eau >= 2 && gameState.nourriture >= 2 && gameState.energie >= 2;
        // Vérifier si le joueur peut se reposer
        static bool CanRest(GameState gameState) => gameState.energie < gameState.maxEnergie;
        // Vérifier si le joueur peut chercher des ressources
        static bool CanSearchResources(GameState gameState) => gameState.eau < gameState.maxEau || gameState.nourriture < gameState.maxNourriture;
        // Mettre à jour l'état après l'action
        static void UpdateGameStateAfterAction(int choix, GameState gameState, Vent ventActuel, bool hasRunThisTurn)
        {
            switch (choix)
            {

                case 1 when CanWalk(gameState):
                    Walk(gameState);
                    oldWalkeurEvent(gameState, 5);
                    BanditEvent(gameState, 5);
                    break;
                case 2 when CanRun(gameState, ventActuel, hasRunThisTurn):
                    Run(gameState);
                    BanditEvent(gameState, 15);
                    break;
                case 3 when CanRest(gameState):
                    Rest(gameState);
                    break;
                case 4 when CanSearchResources(gameState):
                    SearchResources(gameState);
                    oldWalkeurEvent(gameState, 30);
                    break;
                default:
                    Console.WriteLine(text.Error);
                    break;
            }
            DisplayCurrentState(gameState);
        }
        // Actions du joueur
        static void Walk(GameState gameState)
        {
            Console.WriteLine(text.Walk);
            gameState.distanceParcourue += 10000; // Marche 10 000m par jour
            gameState.energie--;
            gameState.eau--;
            gameState.nourriture--;
        }

        static void Run(GameState gameState)
        {
            Console.WriteLine(text.Run);
            gameState.distanceParcourue += 30000; // Course 30 000m par jour
            gameState.energie -= 2;
            gameState.eau -= 2;
            gameState.nourriture -= 2;
        }

        static void Rest(GameState gameState)
        {
            Console.WriteLine(text.Rest);
            gameState.energie = Math.Min(8, gameState.energie + 3);
            gameState.santeMentale = Math.Min(5, gameState.santeMentale + 1);

        }

        static void SearchResources(GameState gameState)
        {
            Console.WriteLine(text.RessourcesSearch);
            Random rand = new Random();
            int chance = rand.Next(0, 100);

            if (chance < 10) // 10% de chance de ne rien trouver
            {
                Console.WriteLine(text.RessourcesEchec);
            }
            else if (chance < 30) // 20% de chance de trouver à la fois de l'eau et de la nourriture
            {
                gameState.eau = Math.Min(5, gameState.eau + 1);
                gameState.nourriture = Math.Min(10, gameState.nourriture + 1);
                Console.WriteLine(text.RessourcesWaterAndFood);
            }
            else if (chance < 65) // 35% de chance de trouver uniquement de l'eau
            {
                gameState.eau = Math.Min(10, gameState.eau + 1);
                Console.WriteLine(text.RessourcesWater);
            }
            else // 35% de chance de trouver uniquement de la nourriture
            {
                gameState.nourriture = Math.Min(10, gameState.nourriture + 1);
                Console.WriteLine(text.RessourcesFood);
            }

            gameState.energie--; // Chercher des ressources consomme de l'énergie
        }
        // Afficher la distance parcourue et l'état de la tempête
        static void DisplayCurrentState(GameState gameState)
        {
            Console.WriteLine($"\nDistance parcourue : {gameState.distanceParcourue} mètres");
            if (gameState.jour >= 5 && gameState.tempeteDistance > 0)
            {
                Console.WriteLine($"🌪 La tempête à parcourue {gameState.tempeteDistance} mètres.");
            }
            Console.WriteLine("=============================================================================================================\n");
        }
        // Mettre à jour l'état du jeu
        static void UpdateGameState(ref GameState gameState)
        {
            gameState.jour++;

            // Vérifier si le joueur a franchi une nouvelle zone (palier de 100 000 mètres)
            int nouvelleZone = gameState.distanceParcourue / 100000 + 1; // Zone 1 pour 0-100000, Zone 2 pour 100001-200000, etc.

            if (nouvelleZone > gameState.zone)
            {
                gameState.zone = nouvelleZone;
                Console.WriteLine($"\nVous avez atteint la zone {gameState.zone} !");
            }

            // Gérer la tempête
            if (gameState.tempeteDistance > 0)
            {
                gameState.tempeteDistance += 15000; // La tempête avance de 15 000m par jour
            }
        }

        static void DailyEvent(GameState gameState)
        {
            Random rand = new Random();
            int chance = rand.Next(0, 101);

            if (chance < 10) // 10% de chance
            {
                ChroneEvent(gameState);
            }
            else if (chance < 15) // 5% de chance (10% à 15%)
            {
                WindStorm(gameState);
                gameState.windStormOccured = true;
            }
            else // 85% de chance (15% à 100%)
            {
                Console.WriteLine(text.NoDailyEvent);
            }
        }

        static void ChroneEvent(GameState gameState)
        {
            Console.WriteLine(text.Chrone);
            gameState.santeMentale--;
        }
        // Événement Bandit
        static void BanditEvent(GameState gameState, int chance)
        {
            Random rand = new Random();
            int banditChance = rand.Next(0, 101); // Valeur entre 0 et 100
            if (banditChance < chance) 
            {
                int choix = Bandit(gameState); // Appel de la méthode BanditEvent
                switch (choix)
                {
                    case 1:
                        paidBandit(gameState);
                        break;
                    case 2:
                        fightBandit(gameState);
                        break;
                    case 3:
                        negotiate(gameState);
                        break;
                    default:
                        Console.WriteLine(text.ErrorBandit);
                        paidBandit(gameState); // Par défaut, payer en cas d'entrée invalide
                        break;
                }
            }
        }

        static int Bandit(GameState gameState)
        {
            Console.WriteLine(text.BanditChoice);
            return int.Parse(Console.ReadLine());
        }

        static void paidBandit(GameState gameState)
        {
            Console.WriteLine(text.BanditPaid);
            gameState.santeMentale--;
            gameState.nourriture--;
            gameState.eau--;
        }

        static void fightBandit(GameState gameState)
        {
            Console.WriteLine(text.BanditFight);
            gameState.energie -= 3;
        }

        static void negotiate(GameState gameState)
        {
            Random rand = new Random();
            int chance = rand.Next(1, 3);
            if (chance == 1)
            {
                Console.WriteLine(text.BanditNegociateGood);
            }
            else
            {
                Console.WriteLine(text.BanditNegociateBad);
                gameState.santeMentale -= 2;
                gameState.nourriture -= 2;
                gameState.eau -= 2;
            }
        }
        // Événement Corps d'ancien marcheur
        static void oldWalkeurEvent(GameState gameState, int chance)
        {
            Random rand = new Random();
            int oldWalkerChance = rand.Next(0, 101);
            if (oldWalkerChance < chance)
            {
                OldWalkeur(gameState);
            }
        }

        static void OldWalkeur(GameState gameState)
        {
            Console.WriteLine(text.OldWalkeur);
            gameState.santeMentale--;
            gameState.eau = Math.Min(10, gameState.eau + 3);
            gameState.nourriture = Math.Min(10, gameState.nourriture + 3);
        }
        // Événement Tempête de vent
        static void WindStorm(GameState gameState)
        {
            Console.WriteLine(text.WindStorm);
            gameState.jour++;
        }
        // Vérifier les conditions de fin de jeu
        static void CheckGameOverConditions(GameState gameState)
        {
            if (gameState.tempeteDistance >= gameState.distanceParcourue)
            {
                Console.WriteLine(text.GameOverStorm);
                gameState.gameOver = true;
                return;
            }
            if (gameState.energie <= 0 || (gameState.eau <= 0 && gameState.nourriture <= 0))
            {
                Console.WriteLine(text.GameOverRessources);
                gameState.gameOver = true;
                return;
            }
            if (gameState.distanceParcourue >= gameState.distanceObjectif)
            {
                Console.WriteLine(text.Win);
                gameState.gameOver = true;
                return;
            }
        }
        // Afficher le résultat final du jeu
        static void DisplayGameResult(GameState gameState)
        {
            Console.WriteLine("\n*** Résumé de votre aventure ***");
            Console.WriteLine($"Zone atteinte : {gameState.zone}");
            Console.WriteLine($"Distance parcourue : {gameState.distanceParcourue} mètres");
            Console.WriteLine($"Jours écoulés : {gameState.jour}");
            Console.WriteLine($"Énergie restante : {gameState.energie}/{gameState.maxEnergie}");
            Console.WriteLine($"Nourriture restante : {gameState.nourriture}/{gameState.maxNourriture}");
            Console.WriteLine($"Eau restante : {gameState.eau}/{gameState.maxEau}");
            Console.WriteLine($"Santé mentale : {gameState.santeMentale}/{gameState.maxSanteMentale}");

            Console.WriteLine(text.End);
            Console.ReadLine();
        }
    }    
} 