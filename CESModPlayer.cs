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
        private int comboTimer = 0; // Timer for resetting the combo
        private const int ComboResetTime = 90; // 1.5 seconds (90 frames at 60 FPS)
        public const int ComboThreshold = 10; // Combo threshold for triggering effects
        public const float ComboDamageMultiplier = 0.1f; // Damage increase per combo count

        public bool useDebug;

        public bool UseCombo => comboCounter > 0;

        public void IncrementComboCounter()
        {
            comboCounter++;
            comboTimer = ComboResetTime;
        }

        public void ResetComboCounter()
        {
            comboCounter = 0;
        }

        public int GetComboCounter()
        {
            return comboCounter;
        }

        public bool UseDebug(bool _useDebug)
        {
            useDebug = _useDebug;

            if(PlayerInput.GetPressedKeys())

            return true;
        }

        public bool HasReachedComboThreshold()
        {
            return comboCounter >= ComboThreshold;
        }

        public override void ResetEffects()
        {
            useDebug = false;
        }

        public override void UpdateDead()
        {
            ResetComboCounter();
        }
    }
}
