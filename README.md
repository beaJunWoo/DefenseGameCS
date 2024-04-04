# 디펜스 게임 개발: 프로젝트 소개


 
# 프로젝트 소개<br/>
* 이 프로젝트는 Unity를 사용하여 게임을 개발하는 작업을 포함하며, 게임 개발 과정에서 생성된 스크립트만을 포함합니다. <br/>
게임은 전략 디펜스 게임으로 플레이어가 제한된 자원을 사용하여 유닛을 전략적으로 배치하고<br/>
일정 시간동안 적의 공격을 방어하는 게임입니다. <br/>
특별한 전략이 없더라도, 승리나 패배 시 얻는 게임 머니를 사용하여 유닛을 구매하거나 강화할 수 있습니다.<br/>
또한 적을 처치함으로써 얻는 돈으로 버프, 스킬을 활용해 강해 질수있습니다.<br/>

# 플레이 영상<br/>
![GIFMaker_me (1)](https://github.com/beaJunWoo/DefenseGameCS/assets/117621575/0ece0713-7778-456e-8e25-8beca8083aa9)

 -**플레이 풀영상 링크(유튜브)**- <br/>
 [![Video Label](http://img.youtube.com/vi/6CsYmcgk3_M/0.jpg)](https://youtu.be/6CsYmcgk3_M)<br/>


# 개발 기간<br/>
* 프로젝트는 약 2개월 반 동안 개발되었으며, 계속해서 업데이트 중입니다.<br/>

# 외부 라이브러리<br/>
-[Goole Play Games plugin](https://github.com/playgameservices/play-games-plugin-for-unity)<br/>
-[Unity Leaderboard](https://github.com/danqzq/unity-leaderboard-creator)<br/>
-[Google Admod](https://github.com/googleads/googleads-mobile-unity)<br/>

구글 플레이게임 로그인기능, 랭킹 기능, 광고및 보상기능 추가<br/>

# 주요기능<br/>
+ **게임 데이터 저장**<br/>
  + 게임 진행에 필수적인 재화 및 구매 상황, 스테이지 클리어 정보 등을 ScriptableObject를 통해 관리합니다. <br/>
   이는 게임 데이터를 효율적으로 다루기 위한 견고하고 유연한 방법을 제공합니다. <br/>
  + 씬 전환 또는 상점을 나갈 때 전체 데이터를 JSON 파일로 저장함으로써,  <br/>
  데이터의 지속성을 보장하고 쉽게 검색할 수 있는 신뢰할 수 있는 방법을 제공합니다.<br/>
 
+ **오브젝트 풀링**<br/>
  + 원활한 모바일 게임 환경을 유지하기 위해 오브젝트 풀링 기법을 사용하여, <br/>
빠르게 파괴되고 재생성되는 적, 총알 등을 최적화했습니다. <br/>

+ **유닛강화**<br/>
  + 시간이 지남에 따라 점점 더 많아지는 적을 상대하기 위해, 구매, 업그레이드, 버프를 활용하여 유닛을 강화할 수 있습니다. <br/>
각 라운드마다 랜덤으로 등장하는 버프를 통해 모든 유닛의 체력 증가, 장전 속도 버프, 데미지 버프, 보호막, 공격 속도 등을 추가할 수 있습니다. <br/>
이러한 요소는 게임에 재미를 추가합니다.<br/>
시간이 지날수록 많아지는 적을 상대하기 위해 구매, 업그레이드, 버프 를 활용하여 강해질수 있습니다.<br/>

  +모든 유닛은 각각의 데미지, 체력, 탄창 등을 가지고 있으며, 유닛의 종류별로 치명타율, 데미지, 보호막 등이 존재하여 다양한 플레이 방향성을 제공합니다.<br/>

# 개발 과정<br/>
**1. 첫 프로젝트 실패**<br/>
> 초기에 Unity에 익숙해지기 위해 2D로 시작하여 다양한 컴포넌트를 탐색하고 테스트하며 여러 기능을 구현하려고 시도했습니다.<br/>
그러나 첫 프로젝트부터 너무 높은 목표를 설정하고 게임 구상 없이 무작위로 기능을 추가하려다 보니,<br/>
 완성도 있는 게임을 만들지 못했습니다.<br/>
결국 개발 중반에 프로젝트를 포기하고, 이 프로젝트를 두 번째 프로젝트로 시작했습니다.<br/>

**3. 업데이트를 고려한 개발**<br/>
> 두 번째 프로젝트로서, 게임 구상을 미리 하고 게임의 방향성을 정해 프로토타입을 제작했습니다.<br/>
프로토타입을 기반으로 프로젝트를 계속 이어나갔으며, 상점 업그레이드, 다양한 맵, 스킬, 버프 등을 추가했습니다.<br/>
그러나 코드를 초기에 유연하게 작성하지 않아 계속해서 이전 코드를 수정해야 하는 문제에 직면했습니다.<br/>
이번 프로젝트를 통해, 업데이트와 기능 추가 같은 유지보수를 용이하게 하는 코드 작성 요령을 배운 것은 <br/>
개발 에서 가장 큰 겸험중 하나였습니다. <br/>
초기에 저는 함수 하나로 모든 것을 해결하려 했던,<br/>
즉 설비각각의 기능이 아닌 마치 공장 전체를 담당하려는 듯한 방식으로 코드를 작성했습니다. 
이런 접근 방식은 효율적이지 못하고 확장성에 큰 제약을 가했으며, 결과적으로 코드를 수정하고 기능을 추가하는 과정에서 많은 어려움을 겪었습니다.<br/>
결국, 추가가 필요한 코드는 모두 용이하게 변경할 수 있도록 수정했습니다.<br/>
이 과정에서 유지보수가 용이한 코드 작성법을 배웠습니다.<br/>
<br/>

**3. 사용자 편의성 향상**<br/>
> 이 게임의 목표는 Google Play 스토어에 첫 작품을 등록하는 것입니다.<br/>
내부 테스트와 비공개 테스트를 진행하며 다양한 피드백을 받았습니다.<br/>
특히, 유닛을 잘못 배치한 경우 수정이 불가능해 사용자들이 불편함을 느꼈습니다.<br/>
유닛 배치 시스템의 일부 코드를 이해하지 못한 채 작성한 것이 문제였습니다.<br/>
코드를 여러 번 수정하고 디버깅하면서, 유닛 배치와 상호작용을 보다 편리하게 하고,<br/>
유저가 쉽게 조작할 수 있도록 컨트롤 범위를 재조정하고 버튼 배치를 변경했습니다. <br/>
선택한 유닛의 UI도 변경하여 어떤 유닛을 조작 중인지 파악하기 쉽게 했습니다.<br/>
아직 완벽한 수준은 아니지만, 지속적으로 편의성을 개선해 나갈 예정입니다.<br/>


  







