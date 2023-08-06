# Scene Interest Management와 Distance Interest Management를 합친 Custom Interest Management 구현

## Motive
기존의 Scene Interest Management는 다른 씬의 오브젝트만을 비활성화 하기 때문에 같은 씬의 모든 오브젝트가 클라이언트에서 브로드캐스팅 됐다.

이는 클라이언트 프레임 드랍의 직접적인 원인이 될 수 있으므로 Distance Interest Management를 적용하여 플레이어와의 거리에 따라 오브젝트를 비활성화 할 필요가 있었다.

하지만 Mirror의 Interest Manager는 하나의 interest만 적용할 수 있기 때문에 Manager를 수정하거나 별도로 Custom Interest Management를 구현해야 했다.

Manager 수정은 서버 전반적인 수정이 필요하므로 Custom Interest Management를 구현하기로 했다.

<br/>

## CustomInterestManagement.cs
Scene Interest Management와 Distance Interest Management를 합친 구현체이다.

서버는 Interest Management에 따라 씬에 표현할 오브젝트를 탐색하고 sceneObjects에 저장하여 클라이언트에 해당 딕셔너리를 전송한다.

Scene Interest Management는 동일한 씬의 모든 오브젝트를 포함하지만 해당 로직에 거리 조건을 추가하여 새로운 Interest Management를 구현하였다.

<br/>

## CustomInterestManagementCustomRange.cs
클라이언트에서 Visit Range를 조절할 수 있는 인터페이스를 제공한다.

브로드캐스팅은 서버에서 처리해야 되는 작업이므로 Command를 통해 클라이언트에서 서버로 visRange값을 전송한다.