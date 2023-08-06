# Unity Custom Editor로 만든 Build Settings
![image](https://github.com/wjdgh9577/Unity3D/assets/50287835/af53fd70-f037-4ae6-9803-9df2b9f35a90)
## Motivation
빌드 플랫폼 및 용도(테스트용, 배포용)에 따라 매번 설정을 바꿔주는 과정이 복잡하여 자동화가 필요하다고 생각했다.

<br/>

## Fields
### 1. Platform/Version text area
개발 플랫폼과 현재 애플리케이션의 버전을 표기한 영역으로 빌드 전 실수를 방지하기 위해 표기함.

### 2. BuildSettingMode
빌드의 용도를 설정할 수 있으며, 설정에 따라 define symbol을 수정하여 전처리가 진행됨.

### 3. DeviceLogin
체크시 자동으로 디바이스 로그인한다.
플랫폼 및 용도에 따라 설정값이 다르기 때문에 별도의 필드로 분류했다.

### 4. SteamEnable, SandboxMode
스팀 플랫폼 전용 필드이다.

SteamEnable 체크시 게임 로직이 스팀 플랫폼에 맞춰 수정된다.

SandboxMode 체크시 결제 비용이 0원이 된다. 스팀 결제 테스트용 필드이다.

### 5. Addressable Build Button
클릭시 자동으로 어드레서블 빌드를 실행한다.
