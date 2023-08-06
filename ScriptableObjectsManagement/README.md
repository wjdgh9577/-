# Unity Custom Editor로 만든 Scriptable Objects Manager

## Motive
코드의 확장성을 확보하고 전략 패턴을 보다 효율적으로 활용하기 위해 데이터 관리 방식을 Table -> Scriptable Object로 변경하기로 결정.

따라서 기존의 Serialized Table Data(json)를 Scriptable Object로 변환하는 기능을 개발하고 자동화할 필요가 있었다.

<br/>

## SerializableClassCollection.cs
Serialized Table Data를 Deserialize할 클래스를 모아둔 스크립트이다.

<br/>

## ScriptableManagement.cs
Table Data를 Deserialize하고 Scriptable Object로 변환하는 기능을 구현한 스크립트이다.

지정된 경로에서 json파일을 불러오고 지정된 경로에 Scriptable Object를 생성한다.

메뉴바에 기능이 노출되도록 기능별로 Attribute를 설정했고 기획자의 요청에 따라 지원하는 기능을 세분화했다.
