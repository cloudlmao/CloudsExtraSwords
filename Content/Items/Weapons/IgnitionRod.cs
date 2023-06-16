using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CloudsExtraSwords.Content.Items.Weapons
{
    internal class IgnitionRod : ModItem
    {
        // Combo Stuff
        private const int ComboBuffDuration = 300; // Duration of the combo buff in frames (5 seconds at 60 FPS)
        private const float FireSpreadRange = 10f; // Range for spreading fire to nearby enemies

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ignition Rod");
            Tooltip.SetDefault("The Ignition Rod channels the essence of fire, empowering your combos with a scorching burn.\n" +
                "Watch as your enemies become engulfed in a relentless blaze, their hopes extinguished with every strike.");
        }

        public override void SetDefaults()
        {
            Item.shoot = ProjectileID.Flames;
            Item.shootSpeed = 10;

            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;

            Item.width = 80;
            Item.height = 80;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.knockBack = 6;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (!target.friendly)
            {
                CESModPlayer modPlayer = player.GetModPlayer<CESModPlayer>();
                modPlayer.IncrementComboCounter(); // Increment the comboCounter when hitting a non-friendly NPC

                // Check if comboCounter reaches 10
                if (modPlayer.HasReachedComboThreshold())
                {
                    ApplyComboEffects(player, target);

                    // Spread fire to nearby enemies
                    SpreadFireToNearbyEnemies(player, target);
                }

                if(modPlayer.useDebug)
                    Main.NewText("Combo: " + modPlayer.GetComboCounter(), Color.Green);

                SpawnComboParticleEffect(target);
            }
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        private void ApplyComboEffects(Player player, NPC target)
        {
            target.AddBuff(BuffID.OnFire, ComboBuffDuration); // Apply On Fire! debuff for a certain duration

            CESModPlayer modPlayer = player.GetModPlayer<CESModPlayer>();
            modPlayer.ResetComboCounter(); // Reset comboCounter to zero
        }

        private void SpreadFireToNearbyEnemies(Player player, NPC target)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.active && npc.Distance(target.Center) <= FireSpreadRange)
                {
                    npc.AddBuff(BuffID.OnFire, ComboBuffDuration); // Apply On Fire! debuff to nearby enemies as well
                }
            }
        }

        private void SpawnComboParticleEffect(NPC target)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.Blood, 0f, -1f, Scale: 1.5f, newColor: Color.Red, Alpha: 100);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            CESModPlayer modPlayer = player.GetModPlayer<CESModPlayer>();
            // Increase the damage based on the comboCounter value
            float damageIncrease = modPlayer.GetComboCounter() * CESModPlayer.ComboDamageMultiplier;
            damage += damageIncrease;

            base.ModifyWeaponDamage(player, ref damage);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddIngredient(ItemID.Fireblossom, 3);
            recipe.AddIngredient(ItemID.FieryGreatsword, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
