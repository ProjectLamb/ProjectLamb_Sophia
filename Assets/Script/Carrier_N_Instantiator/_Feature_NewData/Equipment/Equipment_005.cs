using System.Collections.Generic;

namespace Feature_NewData
{
    /*
    
    돈으로 업하는거 이거 좀 이상하다..
    Money Is Power 이니 이상하지는 않다..
    그렇다면 메소값을 늘 참조하는 방식으로 추딜이 들어가는 기획을 하는게 나을지도.;
    
    */

    public class Equipment_005 {

        public Player playerRef; 

        public void SetPlayer(Player player) {
            playerRef = player;
        }
    }
}