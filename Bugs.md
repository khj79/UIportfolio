# WebGL 빌드 관련 버그
* chrome 개발자 도구 콘솔에 아래와 같은 에러가 뜨면서, 로딩이 되지 않는 상태
```
Uncaught (in promise) TypeError: Cannot read properties of undefined (reading 'match')
    at Object.cacheControl (WebGL.loader.js:1:953)
    at T (WebGL.loader.js:1:40660)
    at P (WebGL.loader.js:1:44684)
    at WebGL.loader.js:1:47416
```
* [해결책](https://discussions.unity.com/t/webgl-application-failing-since-upgrade-to-6000-0-35f1/1594695)
* index.html의 config 부분을 아래와 같이 수정
```
var config = {
        arguments: [],
        dataUrl: buildUrl + "/docs.data",
        frameworkUrl: buildUrl + "/docs.framework.js",
        codeUrl: buildUrl + "/docs.wasm",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "UIPortfolio",
        productVersion: "0.1.0",
        showBanner: unityShowBanner,
        workerUrl: buildUrl + "/docs.data", // ✅ undefined.match 에러 방지
      };
```