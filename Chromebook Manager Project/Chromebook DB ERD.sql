Select 
   I.firstName [First Name],
   IsNull(Left(I.middleName,1),'') [Middle Name],   
   I.lastName [Last Name],
   't' + Cast(P.personID as varchar(12)) [Unique User ID],
------------------------------------------------------
--   Left(Replace(NEWID(),'-',''),14) pwd,
------------------------------------------------------
   --Min('Teacher') [Role],
   --Min(Coalesce(Cast(exc2.val2 as Int),         --Schoology Role Override
   Min(Coalesce(exc2.val2,         --Schoology Role Override
        Case CEA.value 
		   When 'CS' Then '100'     --Communication Staff
		   When 'CR' Then '101'     --Counselor
		   When 'SM' Then '102'     --Student Messaging Staff
		   When 'SA' Then '250911'  --School Administrator
		   When 'TS' Then '103'     --Teacher - School Updates
		   When 'TC' Then '250913'  --Teacher
 	    End,  --Campus schoology Role --Sort of exceptions table managed thru Campus  --  Communications Staff
		Case
		   --When EA.assignmentCode in (931001,931002,932001,932002,933000,933001,933002,933010) Then 'Principal'   --Principal --Gets assigned school admin role
		   When EA.title in ('Counselor') Then '101'
		   When EA.title in ('Principal') Then 'Principal'
		   When EA.title in ('TOSA') Then '250913'
		   When EA.teacher = 1 Then '250913' Else '261129'
	    End)) [Role]
   --Max(Cast(IsNull(Exc.val2,Case Sch.number When '000' Then '2809706' When '013' Then '2809706'  When '103' Then '2809706' When '045' Then '2809706' When '905' Then '2809706' Else Sch.number End) as int)) school 
   --Max(schToUse.schoologyCode) school 
From Robbinsdale.dbo.EmploymentAssignment EA With (noLock)
Left Join Robbinsdale.dbo.CustomEmploymentAssignment CEA with (noLock)
On EA.assignmentID = CEA.assignmentID
And CEA.attributeID = 1454
Inner Join Robbinsdale.dbo.Person P With (noLock)
On EA.personID = P.personID
And (EA.endDate is Null or EA.endDate > GETDATE())
--And EA.teacher = 1
and EA.assignmentID = 16619
Inner Join robbinsdale.dbo.[Identity] I With (noLock)
On P.currentIdentityID = I.identityID
Inner Join Robbinsdale.dbo.UserAccount UA With (noLock)
On P.personID = UA.personID
And (UA.expiresDate is Null or UA.expiresDate >= GETDATE())
And UA.[disable] = 0
Left Join RobbCustom.dbo.tblNRExceptions Exc2
On UA.personID = Exc2.val
And Exc2.area = 'SchoologyRole'
where EA.personID = 119837
Group By 
   I.firstName,
   I.lastName,
   IsNull(Left(I.middleName,1),''),   
   UA.username,
   't' + Cast(P.personID as varchar(12))
