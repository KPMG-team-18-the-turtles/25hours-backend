# API Documentation

## JSON Structure

이 프로그램이 반환하는 JSON 도큐멘트에는 크게 두 가지가 있습니다.
그 중 클라이언트를 나타내는 도큐멘트를 `ClientType`, 회의를 나타내는 도큐멘트를 `MeetingType`이라고 합니다.

### `ClientType` Structure

```js
{
  "name": string,                 // 이름
  "contact": string,              // 연락처 (이메일 주소, 전화번호 등...)
  "profileImagePath": string,     // 프로필 사진 위치
  "meetings": [MeetingType, ...], // 현재까지 진행한 회의 목록
  "agenda": [                     // 할 일 목록
    {
      "content": string,          // 할 일의 내용
      "completed": boolean        // 할 일 완료 여부
    }, ...
  ],
  "id": string (ObjectId)         // 클라이언트 고유 ID
}
```

### `MeetingType` Structure

```js
{
  "index": int,               // 회의 번호
  "date": Date,               // 회의 진행 날짜
  "keywords": [string, ...],  // 키워드 (최대 15개)
  "summary": [string, ...],   // 회의의 요약
  "rawTextLocation": string,  // 텍스트 파일의 위치
  "rawAudioLocation": string, // 오디오 파일의 위치
  "id": null                  // 사용되지 
}
```

## API Reference

여기서부터는 사용 가능한 API에 관한 정보입니다.

> 아래 예시에서 사용되는 `https://25hours.example.com`은 실제 존재하는 도메인이 아니며, 요청 전송의 예시를 위해 사용되었습니다. 실제로 사용할 때는 서버의 도메인으로 바꿔서 요청을 보내 주십시오.

### GET api/clients

데이터베이스에 등록된 모든 클라이언트 정보 전체를 `ClientType` 배열으로 반환합니다.

> ClientType의 정의에 관해서는 위 [ClientType Structure](#clienttype-structure) 절을 참조하십시오.

#### Example

Request:
```
GET https://25hours.example.com/api/clients
```

Response:
```json
[
  {
    "name":"Cafe Man",
    "contact":"010-1234-1234",
    "profileImagePath":"path/to/profile.jpg",
    "meetings":[],
    "agenda":[],
    "id":"defacedcafed00d100c2c001"
  }
]
```

### POST api/clients

새로운 클라이언트를 데이터베이스에 추가합니다.

새로운 클라이언트를 추가하기 위해 필요한 최소한의 정보는 다음과 같습니다.
- 이름 (`name`)
- 연락처 (`contact`)
- 할 일 목록 (`agenda`)

> 클라이언트 생성 과정에서 회의 목록 (`meeting` 프로퍼티)을 채워 넣는 것은 불가능합니다. 클라이언트 생성 이후 [`POST api/clients/{id}/meetings`](#post-apiclientsidmeetings) 요청을 통해 별도로 입력해 주십시오.

#### Example

Request:
```
POST https://25hours.example.com/api/clients

---- Body ----
{
  "name":"Bakery Man",
  "contact":"010-5678-5678",
  "meetings":[],
  "agenda":[],
  "id":"0"
}
```

Response:
```json
{
  "name":"Bakery Man",
  "contact":"010-5678-5678",
  "profileImagePath": null,
  "meetings":[],
  "agenda":[],
  "id":"defacedbace21d00d12c00l2"
}
```

### GET api/clients/{id}

특정 클라이언트에 관한 정보를 얻어옵니다.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |

#### Example

Request:
```
GET https://25hours.example.com/api/clients/defacedcafed00d100c2c001
```

Response:
```
{
  "name":"Cafe Man",
  "contact":"010-1234-1234",
  "profileImagePath":"path/to/profile.jpg",
  "meetings":[],
  "agenda":[],
  "id":"defacedcafed00d100c2c001"
}
```

### POST api/clients/{id}/profile-image

프로필 이미지를 업로드하고 새로운 이미지로 데이터베이스를 업데이트합니다.

프로필 이미지는 Multipart Form Data를 이용해서 요청에 포함시키십시오. 현재 사용 가능한 포맷은 `image/jpeg`로 한정됩니다. 파일을 업로드하기 전 미리 이미지를 적절한 포맷으로 변환해 주십시오.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |

#### Example

Request:
```
POST https://25hours.example.com/api/clients/defacedcafed00d100c2c001/profile-image

---- Body ----
(Multipart binary data)
```

Response:
```json
{
    "count":1,
    "size":54321,
    "path":"random.filename"
}
```

### GET api/clients/{id}/meetings

해당 클라이언트에게 포함된 모든 회의 데이터를 `MeetingType` 배열으로 가져옵니다.

> MeetingType의 정의에 관해서는 위 [MeetingType Structure](#meetingtype-structure) 절을 참조하십시오.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |

#### Example

Request:
```
GET https://25hours.example.com/api/clients/defacedcafed00d100c2c001/meetings
```

Response:
```json
[
  {
    "index":0,
    "date":"2020-01-01",
    "keywords":["Cafe", "Coffee", "Customers", "Return", "Mug"],
    "summary": [
      "For this cafe to be more successful, we need to sell more delicious coffee.",
      "Returning customers must be welcomed even more.",
      "Better looking mugs cannot be ignored, in my opinion."
    ],
    "rawTextLocation": "random.file.path.1.txt",
    "rawAudioLocation": "random.file.path.2.wav",
    "id": null
  },
  {
    "index":1,
    "date":"2020-01-02",
    "keywords":["Advertisement", "Smile", "Funny", "Events"],
    "summary": [
      "Advertisement is important because it makes people to come in the first place.",
      "Smile can improve the customer return rate even further.",
      "Viral events can attract more customers."
    ],
    "rawTextLocation": "random.file.path.3.txt",
    "rawAudioLocation": "random.file.path.4.wav",
    "id": null
  },
]
```

### POST api/clients/{id}/meetings

새로운 회의 엔트리를 생성합니다.

이 요청을 사용해서 회의의 메타데이터와 회의 녹음 데이터를 한꺼번에 전송하는 것은 불가능합니다.
이 요청을 이용해서 먼저 회의 엔트리를 생성한 후, [`POST api/clients/{id}/meetings/{index}/upload-audio`](#post-apiclientsidmeetingsindexupload-audio) 요청을 이용해서 별도로 음성 파일을 업로드해 주십시오.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |

#### Example

Request:
```
POST https://25hours.example.com/api/clients/defacedcafed00d100c2c001/meetings

---- Body ----
{
  "index": 0
}
```

Response:
```json
{
  "index":2,
  "date":"2020-01-03",
  "keywords":[],
  "summary": [],
  "rawTextLocation": "",
  "rawAudioLocation": "",
  "id": null
},
```

### GET api/clients/{id}/meetings/{index}

해당 인덱스의 회의 정보를 가져옵니다.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |
| `index` | int | 가져올 회의의 인덱스입니다. 0부터 시작하는 값으로, 항상 가장 최근에 한 회의가 가장 큰 인덱스를 갖습니다. |

#### Example

Request:
```
GET https://25hours.example.com/api/clients/defacedcafed00d100c2c001/meetings/0
```

Response:
```json
{
  "index":0,
  "date":"2020-01-01",
  "keywords":["Cafe", "Coffee", "Customers", "Return", "Mug"],
  "summary": [
    "For this cafe to be more successful, we need to sell more delicious coffee.",
    "Returning customers must be welcomed even more.",
    "Better looking mugs cannot be ignored, in my opinion."
  ],
  "rawTextLocation": "random.file.path.1.txt",
  "rawAudioLocation": "random.file.path.2.wav",
  "id": null
}
```

### POST api/clients/{id}/meetings/{index}/upload-audio

음성 회의록을 업로드합니다. 업로드된 회의록은 자동으로 분석되어 해당 회의 데이터를 갱신합니다.

서버 리소스의 한계로 업로드된 음성 파일의 변환은 불가능합니다. 업로드하기 전에 오디오 파일의 포맷인 `audio/x-wav`인지 다시 한 번 확인해 주십시오. 만약 올바르지 않은 오디오 파일이 업로드된다면 텍스트 분석이 되지 않습니다.

#### Parameters

| Parameter name | Type | Description |
| ----- | ----- | ----- |
| `id` | string(ObjectId) | 가져올 클라이언트의 ID입니다. 24자리의 16진수 숫자를 문자열로 나타낸 값이 주어져야 합니다. |
| `index` | int | 가져올 회의의 인덱스입니다. 0부터 시작하는 값으로, 항상 가장 최근에 한 회의가 가장 큰 인덱스를 갖습니다. |

#### Example

Request:
```
POST https://25hours.example.com/api/clients/defacedcafed00d100c2c001/meetings/2/upload-audio

---- Body ----
(Multipart binary data)
```

Response:
```json
{
    "count":1,
    "size":8249263,
    "path":"random.filename"
}
```
