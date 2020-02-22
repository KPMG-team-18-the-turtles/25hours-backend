# 25 Hours

> "숨겨진 한 시간을 찾아서"

2020 KPMG 한국 해커톤에 출전한 18팀 **The Turtles**의 공식 백엔드 리포지토리입니다.

## 사용 가능한 API 목록

REST API를 통해 클라이언트와 회의 내용을 조정할 수 있습니다. 더 자세한 설명은 `docs/` 폴더 안의 [API 레퍼런스](https://github.com/KPMG-team-18-the-turtles/25hours-backend/blob/master/docs/API.md)를 참고하십시오.

| API | 설명 |
| --- | ---- |
| `GET api/clients` | 현재 존재하는 모든 클라이언트 정보를 조회하여 반환합니다. |
| `POST api/clients` | 새로운 클라이언트를 만들어 데이터베이스에 저장합니다. |
| `GET api/clients/{id}` | 특정 클라이언트의 정보를 반환합니다. `id`는 MongoDB `ObjectId` 타입으로, 24자리의 16진수 문자열입니다. |
| `DELETE api/clients/{id}` | 특정 클라이언트를 데이터베이스에서 삭제합니다. |
| `GET api/clients/{id}/meetings` | 지금까지 데이터베이스에 저장된 모든 회의 정보를 반환합니다. |
| `GET api/files/{filename}` | 서버에 저장된 파일을 가져옵니다. .wav와 .txt 파일을 가져올 수 있습니다. |

## Technical Information

### Dependencies

이 서버를 실행하기 위해서는 다음 프로그램/라이브러리가 필요합니다.

- Visual Studio 2019
- IIS Express 10.0
- .NET Core (=2.2)
- ASP.NET Core (=2.2)
- MongoDB (>=4.2)
- OpenTextSummarizer.NET (=1.0)

### Build Instructions

먼저 리포지토리를 클론하거나 ZIP 아카이브를 다운로드합니다.

```sh
$ git clone git+https://github.com/KPMG-team-18-the-turtles/25hours-backend
```

그 다음, MongoDB 인스턴스가 켜져 있는지 확인하십시오. 만약 그렇지 않다면, MongoDB 데몬을 실행합니다.

```sh
$ mongod --dbpath /path/to/database/folder
```

MongoDB 쉘을 실행해서 필요한 데이터베이스를 생성합니다.

```sh
$ mongo
> use ClientDatabase
> db.createCollection("Clients")
```

마지막으로, Visual Studio로 `src/api-server.sln` 솔루션을 열고, `api-server` 프로젝트를 빌드합니다.

### Notes

.NET Core 2.2의 버그로 인해, 리버스 프록시를 설정하지 않고 ASP.NET Core 서버에 접속할 경우 `StackOverflowException`이 발생합니다.
(관련 정보는 [dotnet/aspnetcore#8137](https://github.com/dotnet/aspnetcore/issues/8137) 이슈를 참조하십시오.)
따라서 서버에 배포할 때는 항상 리버스 프록시를 설정해 주십시오. 개발 환경용 리버스 프록시는 IIS Express 10.0 버전을 추천합니다.

만약 텍스트가 제대로 요약되지 않거나, `OpenTextSummarizer`에서 예외가 발생한다면, 언어 사전이 제대로 설정되어 있는지 확인하십시오.
`~/.nuget/


## License

이 리포지토리의 모든 소스코드는 [GNU General Public License version 2](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html)에 따라 배포됩니다. 자세한 정보는 리포지토리의 [`LICENSE.md`](https://github.com/KPMG-team-18-the-turtles/25hours-backend/blob/master/LICENSE) 파일을 참조하십시오.
