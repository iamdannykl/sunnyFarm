using Godot;
using System;
using SunnyFarm.code;

public partial class buy : Button
{
    public EquipInfo equipInfo;

    public void _on_BuyButton_pressed()
    {
        if (Text.Length <= 0) return;
        if (BasicsCore.Instance.playerWeapons.Count >= 6 || BasicsCore.Instance.MoneyValue < Text.ToInt() || equipInfo == null)
        {
            GD.Print("You do not have enough money or you have more than 6 weapons");
        }
        else
        {
            if (!equipInfo.isProps)
            {
                BasicsCore.Instance.MoneyValue -= Text.ToInt();
                Panel panel = GetParent<Panel>();
                BasicsCore.Instance.addWeaponsFromShop(equipInfo);
                panel.GetNode<TextureRect>("TextureRect").Texture = null;
                panel.GetNode<Label>("name").Text = null;
                panel.GetNode<Label>("describe").Text = null;
                Text = null;
                panel.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
                equipInfo = null;
            }
            else
            {
                BasicsCore.Instance.MoneyValue -= Text.ToInt();
                Panel panel = GetParent<Panel>();
                //player.Instance.addWeaponsFromShop(equipInfo);
                panel.GetNode<TextureRect>("TextureRect").Texture = null;
                panel.GetNode<Label>("name").Text = null;
                panel.GetNode<Label>("describe").Text = null;
                Text = null;
                panel.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
                equipInfo = null;
            }
        }
    }
}