using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CloudsExtraSwords.Content.Items.Weapons
{
    internal class IronwoodPolearm : ModItem
    {
        // Combo Stuff
        private const int ComboBuffDuration = 300; // Duration of the combo buff in frames (5 seconds at 60 FPS)
        private const float FireSpreadRange = 10f; // Range for spreading fire to nearby enemies

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ironwood Polearm");
            Tooltip.SetDefault("Forged from enchanted ironwood, a weapon of unrivaled strength and mystical allure.");
        }

        public override void SetDefaults()
        {
            //Item.shoot = ProjectileID.Starfury;
            //Item.shootSpeed = 10;

            Item.damage = 23;
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

                Main.NewText("Combo: " + modPlayer.GetComboCounter(), Color.Green); // Print the combo counter in chat for now, later i will create an accessory that will show Combo information.

                SpawnComboParticleEffect(target);
            }
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
                Dust.NewDust(target.position, target.width, target.height, DustID.Iron, 0f, -1f, Scale: 1.5f, Alpha: 100);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            CESModPlayer modPlayer = player.GetModPlayer<CESModPlayer>();
            // Increase the damage based on the comboCounter value
            float damageIncrease = modPlayer.GetComboCounter() * CESModPlayer.ComboDamageMultiplier;
            damage += damageIncrease;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddIngredient(ItemID.IronBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
