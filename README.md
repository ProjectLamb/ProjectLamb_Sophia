# Sophia Prototype Version 1.0

> Sophia_P_V_1_0 프로젝트 생성

#### 목차
1. [Git Setting](#1-git-setting)
2. [Git 협업시 지켜야 하는것들](#2-git-협업시-지켜야-하는것들)

---

## 1. Git Setting

### 0. 유니티 프로젝트 만들기

**① Clone 하기**
```bash
git clone https://github.com/ProjectLamb/ProjectLamb_Sophia.git
git fetch
```

**② 유니티 프로젝트 만들기**
<div align=center>
    <img src="/img/2023-03-31-10-34-10.png">
    <p>1. 3D(URP) 선택 <br>2. "Sophia_P_V_1_0" 으로 생성</p>
</div>

**③ 유니티 프로젝트에 clone한 내용을 그대로 대입한다.**


---
### 1. git lfs

<div align=center>
    <img src="/img/2023-03-31-09-57-01.png" width=250px>
</div>

git lfs 설정을 한다면 **단일 파일 최대 100MB** 까지의 데이터도 협업할 수 있다. 왜 한것이냐면. 혹시모를 용량 큰 파일때문에 push가 안되는거 사전 예방하려고..

**1. git lfs 설치**

[Git 대형 파일 스토리지 설치 - GitHub Docs](https://docs.github.com/ko/repositories/working-with-files/managing-large-files/installing-git-large-file-storage)

**2. 활성화 하기**

100mb 이상인 파일, 폴더가 있으면 track 으로 지정해줘야 한다.
.gitattributes 파일을 만든다.

```.gitattribute
*.zip filter=lfs diff=lfs merge=lfs -text
*.mp4 filter=lfs diff=lfs merge=lfs -text
```

**3. LFS가 적용된 repo clone하기**
git clone을 하고나서 pull이나 fetch를 해도 제대로 lfs가 동작하지 않을때,
이 때는 git lfs pull을 해주어야한다.

```bash
git clone [url]
git lfs pull <-- 이거 해야지 오류가 안생김
```

---

### 2. 유니티 전용 .gitignore 설정

.gitingore 만들고, 다음 텍스트를 .gitingore에 넣기

```.gitignore
# Created by https://www.toptal.com/developers/gitignore/api/unity
# Edit at https://www.toptal.com/developers/gitignore?templates=unity

### Unity ###
# This .gitignore file should be placed at the root of your Unity project directory
#
# Get latest from https://github.com/github/gitignore/blob/main/Unity.gitignore
/[Ll]ibrary/
/[Tt]emp/
/[Oo]bj/
/[Bb]uild/
/[Bb]uilds/
/[Ll]ogs/
/[Uu]ser[Ss]ettings/

# MemoryCaptures can get excessive in size.
# They also could contain extremely sensitive data
/[Mm]emoryCaptures/

# Recordings can get excessive in size
/[Rr]ecordings/

# Uncomment this line if you wish to ignore the asset store tools plugin
# /[Aa]ssets/AssetStoreTools*

# Autogenerated Jetbrains Rider plugin
/[Aa]ssets/Plugins/Editor/JetBrains*

# Visual Studio cache directory
.vs/

# Gradle cache directory
.gradle/

# Autogenerated VS/MD/Consulo solution and project files
ExportedObj/
.consulo/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Unity3D generated meta files
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity3D generated file on crash reports
sysinfo.txt

# Builds
*.apk
*.aab
*.unitypackage
*.app

# Crashlytics generated file
crashlytics-build.properties

# Packed Addressables
/[Aa]ssets/[Aa]ddressable[Aa]ssets[Dd]ata/*/*.bin*

# Temporary auto-generated Android Assets
/[Aa]ssets/[Ss]treamingAssets/aa.meta
/[Aa]ssets/[Ss]treamingAssets/aa/*

# Ignore Plugins
Asset/Plugins

# End of https://www.toptal.com/developers/gitignore/api/unity
```

---

### 3. git window & mac 협업
**LF, CRLF 관련 오류**
서로 다른 OS간 git 사용때문에 발생하는 오류다 정확한것은 
OS에 맞는 설정을 구글에 꼭 찾아보길!
.gitconfig 설정

## 2. Git 협업시 지켜야 하는것들

<div align=center>
    <img src="/img/2023-03-31-10-41-08.png">
    <h4>GitFlow 방식을 통해 브랜치를 나누자</h4>
</div>

1. main은 항상 버젼 업데이트 할때만 Merge 하는것으로!
2. develop : 브랜치를 파서, 협업하는것을 합치는 "대리자 브랜치"로 사용
3. feature/guild : 이것으로 우리 각자의 브랜치를 파는것을 하자

[(알아두면 개발팀장가능) GitFlow vs Trunk-based 협업방식](https://www.youtube.com/watch?v=EV3FZ3cWBp8)