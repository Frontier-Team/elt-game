using UnityEngine;

namespace _Game.Scripts.Interactions
{
    [CreateAssetMenu(menuName = "Game Data/Votes")]
    public class CharacterData : ScriptableObject
    {
        public Sprite characterImage;
        public int voteId = 0;
        public string voteSummary = "5 word summary";
        
        [TextArea(3, 6)]
        public string voteDescription = "Long form description";
        public string characterName = "Character name";
    }
}