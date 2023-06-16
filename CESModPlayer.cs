using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CloudsExtraSwords
{
    internal class CESModPlayer : ModPlayer
    {
        // Combo Settings
        private int comboCounter = 0; // Counter for keeping track of combos
        public const int ComboThreshold = 10; // Combo threshold for triggering effects
        public const float ComboDamageMultiplier = 0.1f; // Damage increase per combo count

        public bool UseCombo => comboCounter > 0;

        public void IncrementComboCounter()
        {
            comboCounter++;
        }

        public void ResetComboCounter()
        {
            comboCounter = 0;
        }

        public int GetComboCounter()
        {
            return comboCounter;
        }

        public bool HasReachedComboThreshold()
        {
            return comboCounter >= ComboThreshold;
        }

        public override void ResetEffects()
        {

        }

        public override void UpdateDead()
        {
            ResetComboCounter(); // Reset the combo counter when the player dies
        }
    }
}
