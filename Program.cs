using System;
using System.Collections.Generic;

namespace CamelGame
{
    // Enum pour représenter les différents types de vents
    enum Vent
    {
        Zefirine,
        Slamino,
        Steche,
        Choon,
        Crivetz,
        Furvent
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Initialisation des variables de départ
            GameState gameState = InitializeGame();
            Rules();
            Console.WriteLine("Appuyez sur Entrée pour commencer le jeu...");
            Console.ReadLine();
            // Boucle principale du jeu
            while (gameState.jour <= 33 && gameState.distanceParcourue < gameState.distanceObjectif && gameState.energie > 0)
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


        static void Rules()
        {
            Console.WriteLine("Règle du jeu :");
        }

        // Initialisation du jeu
        static GameState InitializeGame()
        {
            return new GameState
            {
                jour = 1,
                zone = 1,
                distanceParcourue = 0,
                distanceObjectif = 500000,
                energie = 8,
                maxEnergie = 8,
                nourriture = 10,
                maxNourriture = 10,
                eau = 10,
                maxEau = 10,
                tempeteDistance = 0,
                santeMentale = 5,
                maxSanteMentale = 5,
            };
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
                if (chance <= 60) return Vent.Zefirine;
                if (chance <= 90) return Vent.Slamino;
                return Vent.Steche;
            }
            else if (zone == 2)
            {
                if (chance <= 40) return Vent.Zefirine;
                if (chance <= 70) return Vent.Slamino;
                if (chance <= 90) return Vent.Steche;
                return Vent.Choon;
            }
            else if (zone == 3)
            {
                if (chance <= 20) return Vent.Zefirine;
                if (chance <= 60) return Vent.Slamino;
                if (chance <= 80) return Vent.Steche;
                return Vent.Choon;
            }
            else if (zone == 4)
            {
                if (chance <= 5) return Vent.Zefirine;
                if (chance <= 30) return Vent.Slamino;
                if (chance <= 60) return Vent.Steche;
                return Vent.Choon;
            }
            else
            {
                if (chance <= 1) return Vent.Zefirine;
                if (chance <= 20) return Vent.Choon;
                return (Vent)(rand.Next(4, 6)); // Choon, Crivetz ou Furvent
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
                    Console.WriteLine("Slamino ralentit votre progression. -1 ⚡");
                    gameState.energie--; // Réduit l'énergie de 1 et empêche la course pour 1 action
                    break;
                case Vent.Steche:
                    Console.WriteLine("Stèche vous empêche de courir. -1 ⚡");
                    gameState.energie--; // Réduit l'énergie de 1 et empêche la course pour les 2 actions
                    break;
                case Vent.Choon:
                    Console.WriteLine("Choon vous fatigue beaucoup. -1 ⚡");
                    gameState.energie--; // Réduit l'énergie de 1
                    break;
                case Vent.Crivetz:
                    Console.WriteLine("Crivetz est une tempête, difficile d'avancer ! -2 ⚡");
                    gameState.energie -= 2; // Réduit l'énergie de 2
                    break;
                case Vent.Furvent:
                    Console.WriteLine("Furvent est une tempête violente ! -3 ⚡ -1 🍖");
                    gameState.energie -= 3; // Réduit l'énergie de 3
                    gameState.nourriture--; // Réduit aussi la nourriture de 1
                    break;
            }
        }


        // Afficher les informations sur le vent
        static void DisplayWindInfo(Vent vent)
        {
            string description = vent switch
            {
                Vent.Zefirine => "Zéfirine : Vent neutre, pas d'effet particulier.",
                Vent.Slamino => "Slamino : Vous ralentit, vous ne pouvez courir qu'une fois aujourd'hui.",
                Vent.Steche => "Stèche : Vous ralentit vous ne pouvez pas courir afin d'éviter les débris.",
                Vent.Choon => "Choon : Vent puissant, diminue l'énergie de -1 aujourd'hui.",
                Vent.Crivetz => "Crivetz : Telpête, diminue l'énergie de -2 aujourd'hui.",
                Vent.Furvent => "Furvent : Tempête violente, diminue l'énergie de -3 et la nourriture de -1.",
                _ => "Vent inconnu."
            };
            Console.WriteLine($"* Vent : {description} *");
        }

        // Annonce de la tempête
        static void AnnounceStorm()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("Une tempête s'est levée à votre point de départ !");
            Console.WriteLine("Elle avancera de 15 000 mètres par jour jusqu'au bout du monde.");
            Console.WriteLine("==============================\n");
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
            gameState.isRunning = false;

        }

        // Choisir une action
        static int ChooseAction(GameState gameState, Vent ventActuel, bool hasRunThisTurn)
        {
            Console.WriteLine("\nQue voulez-vous faire ?");
            if (CanWalk(gameState)) Console.WriteLine("1 - Marcher (−1 ⚡, −1 🍖, −1 💧)");
            if (CanRun(gameState, ventActuel, hasRunThisTurn)) Console.WriteLine("2 - Courir (−2 ⚡, −2 🍖, −2 💧)");
            if (CanRest(gameState)) Console.WriteLine("3 - Se reposer (+2 ⚡, +1 🧠)");
            if (CanSearchResources(gameState)) Console.WriteLine("4 - Chercher des ressources (−1 ⚡, +1 🍖 ou/et 💧)");

            return int.Parse(Console.ReadLine());
        }

        // Vérifier si le joueur peut marcher
        static bool CanWalk(GameState gameState) => gameState.eau > 0 && gameState.nourriture > 0 && gameState.energie > 0;

        // Vérifier si le joueur peut courir
        static bool CanRun(GameState gameState, Vent ventActuel, bool hasRunThisTurn) => ventActuel != Vent.Steche && (ventActuel != Vent.Slamino || !hasRunThisTurn) && gameState.eau >= 2 && gameState.nourriture >= 2 && gameState.energie >= 2;

        // Vérifier si le joueur peut se reposer
        static bool CanRest(GameState gameState) => gameState.energie < 5;

        // Vérifier si le joueur peut chercher des ressources
        static bool CanSearchResources(GameState gameState) => gameState.eau < 5 || gameState.nourriture < 5;

        // Mettre à jour l'état après l'action
        static void UpdateGameStateAfterAction(int choix, GameState gameState, Vent ventActuel, bool hasRunThisTurn)
        {
            switch (choix)
            {

                case 1 when CanWalk(gameState):
                    oldWalkeurEvent(gameState, 5);
                    Walk(gameState);
                    Console.WriteLine("Vous avez décidé de marcher.");
                    break;
                case 2 when CanRun(gameState, ventActuel, hasRunThisTurn):
                    Run(gameState);
                    Console.WriteLine("Vous avez décidé de courir.");
                    break;
                case 3 when CanRest(gameState):
                    Rest(gameState);
                    Console.WriteLine("Vous vous reposez et récupérez de l'énergie.");
                    break;
                case 4 when CanSearchResources(gameState):
                    oldWalkeurEvent(gameState, 30);
                    SearchResources(gameState);
                    Console.WriteLine("Vous cherchez des ressources.");
                    break;
                default:
                    Console.WriteLine("\nAction impossible ou choix invalide.");
                    break;
            }
            DisplayCurrentState(gameState);
        }
        
        // Actions du joueur
        static void Walk(GameState gameState)
        {
            gameState.distanceParcourue += 10000; // Marche 10 000m par jour
            gameState.energie--;
            gameState.eau--;
            gameState.nourriture--;
            gameState.isRunning = true;
        }

        static void Run(GameState gameState)
        {
            gameState.distanceParcourue += 30000; // Course 30 000m par jour
            gameState.energie -= 2;
            gameState.eau -= 2;
            gameState.nourriture -= 2;
        }

        static void Rest(GameState gameState)
        {
            gameState.energie = Math.Min(8, gameState.energie + 2);
            gameState.santeMentale = Math.Min(5, gameState.santeMentale + 1);


        }

        static void SearchResources(GameState gameState)
        {
            Random rand = new Random();
            int chance = rand.Next(0, 100);

            if (chance < 10) // 10% de chance de ne rien trouver
            {
                Console.WriteLine("Vous n'avez rien trouvé. ❌");
            }
            else if (chance < 30) // 20% de chance de trouver à la fois de l'eau et de la nourriture
            {
                gameState.eau = Math.Min(5, gameState.eau + 1);
                gameState.nourriture = Math.Min(10, gameState.nourriture + 1);
                Console.WriteLine("Vous avez trouvé de l'eau et de la nourriture ! 💧 🍖");
            }
            else if (chance < 65) // 35% de chance de trouver uniquement de l'eau
            {
                gameState.eau = Math.Min(10, gameState.eau + 1);
                Console.WriteLine("Vous avez trouvé de l'eau ! 💧");
            }
            else // 35% de chance de trouver uniquement de la nourriture
            {
                gameState.nourriture = Math.Min(10, gameState.nourriture + 1);
                Console.WriteLine("Vous avez trouvé de la nourriture ! 🍖");
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
            Console.WriteLine("=================================\n");
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

        static void RandomEvent(GameState gameState)
        {
            Random rand = new Random();
            int jourChance = rand.Next(0, 101); // Valeur entre 0 et 100

            // Événement Bandit
           /* if (gameState.isRunning) // Assurez-vous que le joueur court
            {
                int banditChance = rand.Next(0, 101); // Valeur entre 0 et 100
                if (banditChance < 15) // 15% de chance de croiser des bandits
                {
                    int choix = BanditEvent(gameState); // Appel de la méthode BanditEvent
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
                            Console.WriteLine("Choix invalide, vous êtes attaqué par les bandits !");
                            paidBandit(gameState); // Par défaut, payer en cas d'entrée invalide
                            break;
                    }
                }
            } */
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
                Console.WriteLine("Aucun Event Daily");
            }
        }

        static void ChroneEvent(GameState gameState)
        {
            Console.WriteLine("Vous rencontrez un chrone, il semble vous faire quelque chose...");
            Console.WriteLine("-1 🧠");
            gameState.santeMentale--;
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
            Console.WriteLine("Vous découvrez le corps d'un ancien marcheur. Votre santé mentale diminue. Il lui restait quelque ressources...");
            Console.WriteLine("-1 🧠, +3 💧, +3 🍖");
            gameState.santeMentale--;
            gameState.eau = Math.Min(10, gameState.eau + 3);
            gameState.nourriture = Math.Min(10, gameState.nourriture + 3);
        }

        // Événement Tempête de vent

        static void WindStorm(GameState gameState)
        {
            Console.WriteLine("Une tempête de vent arrive, vous vous réfugiez jusqu'au lendemain.");
            gameState.jour++;
        }

        /*static void BanditEvent(GameState gameState)
        {
            Console.WriteLine("Courir vous rend plus visible, des bandits vous attaque.");
            Console.WriteLine("\nQue voulez-vous faire ?");
            if (paidBandit(gameState)) Console.WriteLine("1 - Payer le droit de vie (−1 🍖, −1 💧, −1 🧠)");
            if (fightBandit(gameState)) Console.WriteLine("2 - Se battre (−3 ⚡)");
            if (negotiate(gameState)) Console.WriteLine("3 - Négocier (1/2 de ne rien perdre ou de perdre le double du droit de vie)");

            return int.Parse(Console.ReadLine());
        }
        static void paidBandit(GameState gameState)
        {
            gameState.santeMentale--;
            gameState.nourriture--;
            gameState.eau--;
        }

        static void fightBandit(GameState gameState)
        {
            gameState.energie -= 3;
        }

        static void negotiate(GameState gameState)
        {
            Random rand = new Random();
            int chance = rand.Next(1, 2);
            if (chance = 1)
            {
                Console.WriteLine("Vous êtes un excellent négociateurs, les bandits vous laisse partir.");
            }
            else
            {
                Console.WriteLine("Les négociations se passe mal, vous payez le double.");
                gameState.santeMentale -= 2;
                gameState.nourriture -= 2;
                gameState.eau -= 2;
            }
        }*/

        // Vérifier les conditions de fin de jeu
        static void CheckGameOverConditions(GameState gameState)
        {
            if (gameState.energie <= 0 || (gameState.eau <= 0 && gameState.nourriture <= 0))
            {
                Console.WriteLine("💀 Vous êtes épuisé et ne pouvez plus continuer. Fin du jeu ! 💀");
                DisplayGameResult(gameState);
                return ;
            }
            if (gameState.tempeteDistance >= gameState.distanceParcourue)
            {
                Console.WriteLine("🌪 La tempête vous a rattrapé. Fin du jeu ! 💀");
                DisplayGameResult(gameState);
                return ;
            }
            if (gameState.distanceParcourue >= gameState.distanceObjectif)
            {
                Console.WriteLine("🎉 Vous avez atteint le bout du monde. Félicitations ! 🎉");
                DisplayGameResult(gameState);
                return ;
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
        }
    }

    // Classe pour gérer l'état du jeu
    class GameState
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
    }
}

