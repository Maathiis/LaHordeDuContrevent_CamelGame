using System;

public static class text
{

    public static string Rules => @"
Règle du jeu :
Parcourez 500 000 mètres à travers cinq zones désertiques en gérant vos ressources : eau, nourriture, énergie et santé mentale. Atteignez l’objectif final avant que la tempête ne vous rattrape.
Chaque jour un vent spécifique changera les conditions météorologiques, adaptez-vous et vos ressources pour survivre, utilisez-les pour marcher, courir, vous reposer ou chercher des ressources.
Conseil : Avancez vite et utilisez correctement vos ressources pour atteindre rapidement le bout du monde. Courage !!";

    public static string Start => "Appuyez sur Entrée pour commencer le jeu...";

    public static string GetWindDescription(Vent vent)
    {
        return vent switch
        {
            Vent.Zefirine => "Zéfirine : Vent neutre, pas d'effet particulier.",
            Vent.Slamino => "Slamino : Vous ralentit, vous ne pouvez courir qu'une fois aujourd'hui.",
            Vent.Steche => "Stèche : Vous ralentit, vous ne pouvez pas courir afin d'éviter les débris.",
            Vent.Choon => "Choon : Vent puissant, diminue l'énergie de -1 aujourd'hui.",
            Vent.Crivetz => "Crivetz : Tempête, diminue l'énergie de -2 aujourd'hui.",
            Vent.Furvent => "Furvent : Tempête violente, diminue l'énergie de -3 et la nourriture de -1.",
            _ => "Vent inconnu."
        };
    }

    public static string Slamino => "Slamino ralentit votre progression. -1 ⚡";

    public static string Steche => "Stèche vous empêche de courir. -1 ⚡";

    public static string Choon => "Choon vous fatigue beaucoup. -1 ⚡";

    public static string Crivetz => "Crivetz est une tempête, difficile d'avancer ! -2 ⚡";

    public static string Furvent => "Furvent est une tempête violente ! -3 ⚡ -1 🍖";

    public static string AnnounceStorm => @"
===============================================================
Une tempête s'est levée à votre point de départ !
Elle avancera de 15 000 mètres par jour jusqu'au bout du monde.
===============================================================";

    public static string Walk => "Vous avez décidé de marcher.";

    public static string Run => "Vous avez décidé de courir.";

    public static string Rest => "Vous vous reposez et récupérez de l'énergie.";

    public static string RessourcesSearch => "Vous cherchez des ressources.";

    public static string RessourcesEchec => "Vous n'avez rien trouvé. ❌";

    public static string RessourcesWaterAndFood => "Vous avez trouvé de l'eau et de la nourriture ! 💧 🍖";

    public static string RessourcesWater => "Vous avez trouvé de l'eau ! 💧 ";

    public static string RessourcesFood => "Vous avez trouvé de la nourriture !  🍖";

    public static string NoDailyEvent => "Aucun Event Journalier";

    public static string Chrone => @"
Vous rencontrez un chrone, il semble vous faire quelque chose...
-1 🧠";

    public static string ErrorBandit => "Choix invalide, vous êtes attaqué par les bandits !";

    public static string BanditChoice => @"
Des bandits vous attaque.
Que voulez-vous faire ?
1 - Payer le droit de vie (−1 🍖, −1 💧, −1 🧠)
2 - Se battre (−3 ⚡)
3 - Négocier (1/2 de ne rien perdre ou de perdre le double du droit de vie)";

    public static string BanditPaid => @"
Vous payez les bandits.
−1 🍖, −1 💧, −1 🧠";

    public static string BanditFight => @"
Vous combattez les bandits.
−3 ⚡";

    public static string BanditNegociateGood => "Vous êtes un excellent négociateurs, les bandits vous laisse partir.";

    public static string BanditNegociateBad => @"
Les négociations se passe mal, vous payez le double.
−2 🍖, −2 💧, −2 🧠";

    public static string OldWalkeur => @"
Vous découvrez le corps d'un ancien marcheur. Votre santé mentale diminue. Il lui restait quelque ressources...
-1 🧠, +3 💧, +3 🍖";

    public static string WindStorm => "Une tempête de vent arrive, vous vous réfugiez jusqu'au lendemain.";

    public static string GameOverRessources => "💀 Vous êtes épuisé et ne pouvez plus continuer. Fin du jeu ! 💀";

    public static string GameOverStorm => "🌪 La tempête vous a rattrapé. Fin du jeu ! 💀";

    public static string Win => "🎉 Vous avez atteint le bout du monde. Félicitations ! 🎉";

    public static string Error => "Action impossible ou choix invalide.";

    public static string End => "Fin du jeu. Appuyez sur Entrée pour quitter.";
}

