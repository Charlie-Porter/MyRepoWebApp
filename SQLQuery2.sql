select * from CredentialModel

truncate table CredentialModel

--drop table CredentialModel


SELECT UserId, ActivationCode FROM CredentialModel WHERE UserId = 1					

SELECT UserId, ActivationCode FROM CredentialModel WHERE UserId = '1'
SELECT UserId, ActivationCode FROM CredentialModel WHERE UserId = '1'

SELECT count(*) FROM CredentialModel WHERE UserId = '1' AND ActivationCode = '8c71b0ce-d8fd-419c-9324-a15513a267ba'


UPDATE CredentialModel SET Verified = 'true' WHERE UserId = '1'