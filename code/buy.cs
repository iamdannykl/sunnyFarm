using Godot;
using System;
using SunnyFarm.code;

public partial class buy : Button
{
    public EquipInfo equipInfo;

    public void _on_BuyButton_pressed()
    {
        if (Text.Length <= 0) return;
        if (player.Instance.playerWeapons.Count >= 6 || player.Instance.MoneyValue < Text.ToInt() || equipInfo == null)
        {
            GD.Print("You do not have enough money or you have more than 6 weapons");
        }
        else
        {
            if (!equipInfo.isProps)
            {
                player.Instance.MoneyValue -= Text.ToInt();
                var panel = GetParent<Panel>();
                player.Instance.addWeaponsFromShop(equipInfo);
                panel.GetNode<TextureRect>("TextureRect").Texture = null;
                panel.GetNode<Label>("name").Text = null;
                panel.GetNode<Label>("describe").Text = null;
                Text = null;
                panel.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
                equipInfo = null;
            }
            else
            {
                player.Instance.MoneyValue -= Text.ToInt();
                var panel = GetParent<Panel>();
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