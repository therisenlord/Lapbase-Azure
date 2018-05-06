<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >

<xsl:key name="keyFollowupDays" match="tblPatientList" use="FollowupDays"/>
<xsl:key name="keyMonth" match="tblPatientList" use="VisitMonthsSinceOperation"/>
<xsl:key name="keyMonthPatientID" match="tblMonth_PatientID" use="VisitMonthsSinceOperation"/>
	<!-- <xsl:for-each select="/dsSchema/tblApproach">
						<xsl:if test="$SurgeryType = SurgeryType">-->
<xsl:key name="keySurgeryType" match ="/dsSchema/tblApproach" use ="SurgeryType"/>
	<!--<xsl:for-each select="/dsSchema/tblCategory">
								<xsl:if test="($SurgeryType = SurgeryType) and ($Approach = Approach)">-->
<xsl:key name="keySurgeryType_Approach" match="/dsSchema/tblCategory" use="concat(SurgeryType,Approach)"/>
<xsl:template match="/">
<html>
	<head>
		<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
	</head>
	<body class="printPreview">
		<div class="summaryByQuarterReport">
			<img src="../../img/print_header_bar.gif" height="8" width="800px" />
			<div class="clr"></div>
			<table class="details" border="0">
				<tr>
					<td style="width:89px">
						<strong>Surgeon:</strong>
					</td>
					<td style="width:376px">
						
						<xsl:value-of select='translate(dsSchema/tblPatientList/DoctorName_Title, "`", "&apos;")'/>
					</td>
					<td rowspan="4" style="width:313px">
						<h2>Summary Statistics</h2>
						<p class="printDate">
							<xsl:value-of select='dsSchema/tblPatientList/ReportDate'/>
						</p>
					</td>
				</tr>
				<tr>
					<td>
						<strong>Hospital:</strong>
					</td>
					<td>
						<xsl:value-of select='translate(dsSchema/tblPatientList/HospitalName_Title, "`", "&apos;")'/>
					</td>
				</tr>
				<tr>
					<td>
						<strong>Surgery:</strong>
					</td>
					<td></td>
				</tr>
				<tr>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
			</table>

			<table class="data" border="0">
				<xsl:if test="/dsSchema/tblPatientList/ReportName='SummaryByQuarter'">
					<tr>
						<td>
							<table border="0" style="width:800px">
								<tr>
									<td colspan="11">
										<h3>
											Numbers <xsl:value-of select="count(/dsSchema/tblPatientID/PatientID)"/>
										</h3>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<xsl:if test="/dsSchema/tblPatientList/PatientDetailFlag = 1">
						<tr >
							<td>
								<table style="width:800px">
									<tr>
										<td style="width:26px">
											<strong>ID</strong>
										</td>
										<td style="width:136px">Patient Name</td>
										<td style="width:30px">AGE</td>
										<td style="width:85px">Surgery Date</td>
										<td style="width:63px; text-align:right">Start Weight (<xsl:value-of select="/dsSchema/tblPatientList/WeightMeasurment"/>)
										</td>
										<td style="text-align:right; width:70px">
											Current Weight (<xsl:value-of select="/dsSchema/tblPatientList/WeightMeasurment"/>)
										</td>
										<td style="text-align:right; width:65px">Loss</td>
										<td style="text-align:right; width:73px">Init BMI</td>
										<td style="text-align:right; width:65px">BMI</td>
										<td style="text-align:right; width:62px">BMI Change</td>
										<td style="text-align:right; width:55px">%EWL</td>
									</tr>
								</table>
							</td>
						</tr>
					</xsl:if>

					<xsl:for-each select="/dsSchema/tblMonth">
						<xsl:sort data-type="number" order="ascending" select="VisitMonthsSinceOperation"/>
						<xsl:variable name="Months" select="VisitMonthsSinceOperation"/>
						<xsl:if test="/dsSchema/tblPatientList/PatientDetailFlag = 1">
							<tr>
								<td>
									<table style="width:800px">
										<tr>
											<td colspan="4">
												<h3>
													<xsl:call-template name="CheckMonths" >
														<xsl:with-param name="MonthNo" select="$Months"/>
													</xsl:call-template>
												</h3>
											</td>
											<td colspan="7">
												<xsl:if test="VisitMonthsSinceOperation = 1">
													Note:  this will include Patients with no post op follow up visits
												</xsl:if>
											</td>
										</tr>
										<xsl:call-template name="MakePatientDetailRow">
											<xsl:with-param name="MonthNo" select="$Months"/>
										</xsl:call-template>
									</table>
								</td>
							</tr>
						</xsl:if>

						<xsl:if test="/dsSchema/tblPatientList/EWLFlag = 1">
							<tr>
								<td style="height:23px">
									<table border="0" style="width:800px">
										<tr>
											<td colspan="10">
												<div class="greyBlock">&#160;</div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<xsl:call-template name="MakeEWLFooterData">
								<xsl:with-param name="Months" select="$Months"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<tr>
					<td>
						<table border="0" style="width:800px">
							<tr>
								<td colspan="10">
									<div class="greyBlock">&#160;</div>
									<br/>
								</td>
							</tr>
							<tr>
								<td width="156px">
									<h4>&#160;</h4>
								</td>
								<td class="finalHighlight" style="width:100px">
									<strong>Average</strong>
								</td>
								<td class="finalHighlight" style="text-align:center;width:40px">Age</td>
								<td class="finalHighlight" style="text-align:center;width:80px">Start Weight</td>
								<td class="finalHighlight" style="text-align:center;width:60px">Current</td>
								<td class="finalHighlight" style="text-align:center;width:50px">Weight</td>
								<td class="finalHighlight" style="text-align:center;width:70px">Intial</td>
								<td class="finalHighlight" style="text-align:center;width:45px">Current BMI</td>
								<td class="finalHighlight" style="text-align:center;width:85px">BMI Change</td>
								<td class="finalHighlight" style="text-align:center;width:50px">%EWL</td>
							</tr>

							<tr>
								<xsl:variable name ="avgStartWeight" select="format-number(sum(/dsSchema/tblPatientList/StartWeight) div count(/dsSchema/tblPatientList/DateSeen), '##.0')"/>
								<xsl:variable name ="avgWeight" select="format-number(sum(/dsSchema/tblPatientList/Weight) div count(/dsSchema/tblPatientList/DateSeen), '##.0')"/>
								<xsl:variable name ="avgInitBMI" select="format-number(sum(/dsSchema/tblPatientList/InitBMI) div count(/dsSchema/tblPatientList/DateSeen), '##.0')"/>
								<xsl:variable name ="avgBMI" select="format-number(sum(/dsSchema/tblPatientList/BMI) div count(/dsSchema/tblPatientList/DateSeen), '##.0')"/>
								<td>&#160;</td>
								<td class="greyTxt">&#160;</td>
								<td style="text-align:center">
									<xsl:value-of select="format-number(sum(/dsSchema/tblPatientID/AGE) div count(/dsSchema/tblPatientID/PatientID), '##')"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="$avgStartWeight"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="$avgWeight"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="format-number($avgStartWeight - $avgWeight, '##.0')"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="$avgInitBMI"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="$avgBMI"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="format-number($avgInitBMI - $avgBMI, '##.0')"/>
								</td>
								<td style="text-align:center">
									<xsl:value-of select="format-number(sum(/dsSchema/tblPatientList/EWLL) div count(/dsSchema/tblPatientList/DateSeen), '##.0')"/>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>

			<h2>TOTAL GROUP INCLUDED</h2>

			<table border="0" style="width:732px">
				<tr>
					<td style="text-align:right;width:20%">
						<strong>Numbers in Group</strong>
					</td>
					<td style="width:35%">3</td>
					<td style="width:9%">
						<strong>Surgeon</strong>
					</td>
					<td style="width:36%">
						<xsl:value-of select='translate(dsSchema/tblPatientList/DoctorName_Title, "`", "&apos;")'/>
					</td>
				</tr>
				<tr>
					<td style="text-align:right">
						<strong>Exclude</strong>
					</td>
					<td></td>
					<td>
						<strong>Hospital</strong>
					</td>
					<td><xsl:value-of select='translate(dsSchema/tblPatientList/HospitalName_Title, "`", "&apos;")'/></td>
				</tr>
				<tr>
					<td style="text-align:right">
						<strong>Lost to FU</strong>
					</td>
					<td></td>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
				<tr>
					<td style="text-align:right">&#160;</td>
					<td>&#160;</td>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
				<tr>
					<td style="text-align:right">&#160;</td>
					<td>&#160;</td>
					<td>
						<strong>Group</strong>
					</td>
					<td>&#160;</td>
				</tr>
				<tr>
					<td style="text-align:right">&#160;</td>
					<td>&#160;</td>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
			</table>

			<xsl:variable name="Num1m" select="count(/dsSchema/tblPatientID[(FollowupDays &gt;= 0) and (FollowupDays &lt;= 40)])"/>
			<xsl:variable name="Num1mO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt;= 0) and (FollowupDays &lt;= 40)])"/>
			
			<xsl:variable name="Num3m" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 40) and (FollowupDays &lt;= 110)])"/>
			<xsl:variable name="Num3mO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 40) and (FollowupDays &lt;= 110)])"/>
			
			<xsl:variable name="Num6m" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 111) and (FollowupDays &lt;= 180)])"/>
			<xsl:variable name="Num6mO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 111) and (FollowupDays &lt;= 180)])"/>
			
			<xsl:variable name="Num9m" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 181) and (FollowupDays &lt;= 270)])"/>
			<xsl:variable name="Num9mO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 181) and (FollowupDays &lt;= 270)])"/>
			
			<xsl:variable name="Num18m" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 365) and (FollowupDays &lt;= 545)])"/>
			<xsl:variable name="Num18mO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 365) and (FollowupDays &lt;= 545)])"/>

			<xsl:variable name="Num1y" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 270) and (FollowupDays &lt;= 365)])"/> 
			<xsl:variable name="Num1yO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 270) and (FollowupDays &lt;= 365)])"/>

			<xsl:variable name="Num2y" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 545) and (FollowupDays &lt;= 730)])"/> 
			<xsl:variable name="Num2yO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 545) and (FollowupDays &lt;= 730)])"/>
			
			<xsl:variable name="Num3y" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 731) and (FollowupDays &lt;= 1095)])"/> 
			<xsl:variable name="Num3yO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 731) and (FollowupDays &lt;= 1095)])"/>

			<xsl:variable name="Num4y" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 1095) and (FollowupDays &lt;= 1460)])"/>  
			<xsl:variable name="Num4yO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 1095) and (FollowupDays &lt;= 1460)])"/>

			<xsl:variable name="Num5y" select="count(/dsSchema/tblPatientID[(FollowupDays &gt; 1460) and (FollowupDays &lt;= 1825)])"/>   
			<xsl:variable name="Num5yO" select="count(/dsSchema/tblPWDList[(FollowupDays &gt; 1460) and (FollowupDays &lt;= 1825)])"/>

			<xsl:variable name="Num6y" select="count(/dsSchema/tblPatientID[FollowupDays &gt; 1825])"/>   
			<xsl:variable name="Num6yO" select="count(/dsSchema/tblPWDList[FollowupDays &gt; 1825])"/>

			<table border="0" style="width:800px">
				<tbody>
					<tr>
						<td rowspan="2" class="border" style="width:392px">
							<h3>Follow Up Data</h3>
							<table border="0" width="100%">
								<tbody>
									<tr >
										<td>
											<strong>Total</strong>
										</td>
										<td style="text-align:center">
											<xsl:value-of select="$Num1m + $Num3m + $Num6m + $Num9m + $Num1y + $Num18m + $Num2y + $Num3y + $Num4y + $Num5y + $Num6y"/>
										</td>
										<td style="text-align:center">
											<xsl:value-of select="$Num1mO + $Num3mO + $Num6mO + $Num9mO + $Num1yO + $Num18mO + $Num2yO + $Num3yO + $Num4yO + $Num5yO + $Num6yO"/>
										</td>
									</tr>
									<tr>
										<td>&#160;</td>
										<td style="text-align:center">Followed up</td>
										<td style="text-align:center">operated on</td>
									</tr>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'past 1 mth'"/>
										<xsl:with-param name="FollowedUp" select="$Num1m"/>
										<xsl:with-param name="OperatedOn" select="$Num1mO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'past 3 mths'"/>
										<xsl:with-param name="FollowedUp" select="$Num3m"/>
										<xsl:with-param name="OperatedOn" select="$Num3mO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'3 to 6 mths'"/>
										<xsl:with-param name="FollowedUp" select="$Num6m"/>
										<xsl:with-param name="OperatedOn" select="$Num6mO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'6 to 9 mths'"/>
										<xsl:with-param name="FollowedUp" select="$Num9m"/>
										<xsl:with-param name="OperatedOn" select="$Num9mO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'9 - 12 mths'"/>
										<xsl:with-param name="FollowedUp" select="$Num1y"/>
										<xsl:with-param name="OperatedOn" select="$Num1yO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'12 - 18 mths'"/>
										<xsl:with-param name="FollowedUp" select="$Num18m"/>
										<xsl:with-param name="OperatedOn" select="$Num18mO"/>
									</xsl:call-template>
									
									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'1 to 2 years'"/>
										<xsl:with-param name="FollowedUp" select="$Num2y"/>
										<xsl:with-param name="OperatedOn" select="$Num2yO"/>
									</xsl:call-template>
									
									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'2 to 3 years'"/>
										<xsl:with-param name="FollowedUp" select="$Num3y"/>
										<xsl:with-param name="OperatedOn" select="$Num3yO"/>
									</xsl:call-template>
									
									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'3 to 4 years'"/>
										<xsl:with-param name="FollowedUp" select="$Num4y"/>
										<xsl:with-param name="OperatedOn" select="$Num4yO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'4 to 5 years'"/>
										<xsl:with-param name="FollowedUp" select="$Num5y"/>
										<xsl:with-param name="OperatedOn" select="$Num5yO"/>
									</xsl:call-template>

									<xsl:call-template name="Followupdata">
										<xsl:with-param name="Title" select="'&gt; 5 years'"/>
										<xsl:with-param name="FollowedUp" select="$Num6y"/>
										<xsl:with-param name="OperatedOn" select="$Num6yO"/>
									</xsl:call-template>
								</tbody>
							</table>
						</td>
						<td class="border" style="height:98px; vertical-align:top;width:392px">
							<h3>Demographic Data</h3>
							<xsl:variable name="cntFemale" select="count(/dsSchema/tblPatientID[Sex = 'F'])"/>
							<xsl:variable name="cntMale" select="count(/dsSchema/tblPatientID[Sex = 'M'])"/>
							<xsl:variable name="cntOthers" select="count(/dsSchema/tblPatientID[(Sex != 'M') and (Sex != 'F')])"/>
							
							<table border="0" style="width100%">
								<tbody>
									<tr>
										<td colspan="7">Average Age: </td>
									</tr>
									<tr>
										<td style="width:28px">Sex: </td>
										<td style="width:45px">
											<strong>Females</strong>
										</td>
										<td style="width:26px">
											<strong><xsl:value-of select ="$cntFemale"/></strong>
										</td>
										<td style="width:50px">
											%<xsl:value-of select="format-number(100* $cntFemale div ($cntFemale + $cntMale), '##')"/>
										</td>
										<td style="width:60px">
											<strong>Male</strong>
										</td>
										<td style="width:35px">
											<strong><xsl:value-of select ="$cntMale"/></strong>
										</td>
										<td style="width:96px">
											<strong>Other : </strong>
											<strong>
												<xsl:value-of select ="$cntOthers"/>
											</strong>
										</td>
									</tr>
									<tr>
										<td>&#160;</td>
										<td>&#160;</td>
										<td>&#160;</td>
										<td>&#160;</td>
										<td>&#160;</td>
										<td>&#160;</td>
										<td>&#160;</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td class="border" style="vertical-align:top">
							<h3>BMI Distribution</h3>
							<xsl:variable name="avgInitBMI" select="sum(/dsSchema/tblPatientList/InitBMI) div count(/dsSchema/tblPatientList/PatientID)"/>
							<xsl:variable name="minInitBMI">
								<xsl:for-each select="/dsSchema/tblPatientList">
									<xsl:sort data-type="number" select="InitBMI" order="ascending"/>
									<xsl:if test="position() = 1">
										<xsl:value-of select="InitBMI"/>
									</xsl:if>
								</xsl:for-each>
							</xsl:variable>

							<xsl:variable name="maxInitBMI">
								<xsl:for-each select="/dsSchema/tblPatientList">
									<xsl:sort data-type="number" select="InitBMI" order="descending"/>
									<xsl:if test="position() = 1">
										<xsl:value-of select="InitBMI"/>
									</xsl:if>
								</xsl:for-each>
							</xsl:variable>
							
							<table border="0" style="width:100%">
								<tbody>
									<tr style="text-align:center">
										<td >
											<h4>Min <xsl:value-of select="$minInitBMI"/>
										</h4>
										</td>
										<td >
											<h4>
												Mean <xsl:value-of select="format-number($avgInitBMI, '##.0')"/> 
											</h4>
										</td>
										<td >
											<h4>Max <xsl:value-of select="$maxInitBMI"/>
										</h4>
										</td>
									</tr>
									
									<xsl:variable name="BMI35" select="count(/dsSchema/tblPatientList[number(InitBMI) &lt; 35])"/>
									<xsl:variable name="BMI35_39" select="count(/dsSchema/tblPatientList[(number(InitBMI) &gt;= 35) and (number(InitBMI) &lt; 40)])"/>
									<xsl:variable name="BMI40_49" select="count(/dsSchema/tblPatientList[(number(InitBMI) &gt;= 40) and (number(InitBMI) &lt; 50)])"/>
									<xsl:variable name="BMI50_59" select="count(/dsSchema/tblPatientList[(number(InitBMI) &gt;= 50) and (number(InitBMI) &lt; 60)])"/>
									<xsl:variable name="BMI60_69" select="count(/dsSchema/tblPatientList[(number(InitBMI) &gt;= 60) and (number(InitBMI) &lt; 70)])"/>
									<xsl:variable name="BMI70" select="count(/dsSchema/tblPatientList[ number(InitBMI) &gt;= 70])"/>
									<xsl:variable name="BMITotal" select="$BMI35+$BMI35_39+$BMI40_49+$BMI50_59+$BMI60_69+$BMI70"/>
									<tr>
										<td align="center">
											<strong>BMI &lt; 35</strong>
											<br/><xsl:value-of select="$BMI35"/><br/>
											%<xsl:value-of select="format-number(100* $BMI35 div $BMITotal, '##.0')"/>
										</td>
										<td align="center">
											<strong>BMI 35-39</strong>
											<br/><xsl:value-of select="$BMI35_39"/><br/>%<xsl:value-of select="format-number(100* $BMI35_39 div $BMITotal, '##.0')"/>
										</td>
										<td align="center">
											<strong>BMI 40-49</strong>
											<br/><xsl:value-of select="$BMI40_49"/><br/>%<xsl:value-of select="format-number(100* $BMI40_49 div $BMITotal, '##.0')"/>
										</td>
									</tr>
									<tr>
										<td align="center">
											<strong>BMI 50-59</strong>
											<br/><xsl:value-of select="$BMI50_59"/><br/>%<xsl:value-of select="format-number(100* $BMI50_59 div $BMITotal, '##.0')"/>
										</td>
										<td align="center">
											<strong>BMI 60-69</strong>
											<br/><xsl:value-of select="$BMI60_69"/><br/>%<xsl:value-of select="format-number(100* $BMI60_69 div $BMITotal, '##.0')"/>
										</td>
										<td align="center">
											<strong>BMI &gt; 70</strong>
											<br/><xsl:value-of select="$BMI70"/><br/>%<xsl:value-of select="format-number(100* $BMI70 div $BMITotal, '##.0')"/>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>

			<h2><br/>Operation Data</h2>
			<p>This data represents the total number operated on</p>


			<table class="operationData" border="0" style="width:400px">
				<xsl:for-each select="/dsSchema/tblSurgery">
					<xsl:variable name ="SurgeryType" select="SurgeryType"/>
					<xsl:variable name ="cntSurgery" select="count(/dsSchema/tblOperations[SurgeryType = $SurgeryType])"/>
					<tr>
						<td colspan="3" class="surgeryType">
							<em><strong><xsl:value-of select="SurgeryType_Desc"/></strong></em>
						</td>
						<td class="surgeryType" style="width:20px">
							<strong><xsl:value-of select="$cntSurgery"/></strong>
						</td>
					</tr>
					<!--<xsl:key name="keySurgeryType" match ="/dsSchema/tblApproach" use ="SurgeryType"/>-->
					<xsl:for-each select="key('keySurgeryType', $SurgeryType)">
					<!--
					<xsl:for-each select="/dsSchema/tblApproach">
						<xsl:if test="$SurgeryType = SurgeryType">-->
							<xsl:variable name="Approach" select="Approach"/>
							<xsl:variable name ="cntApproach" select="count(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach)])"/>
							<tr>
								<td style="width:15px">&#160;</td>
								<td colspan="2" class="approach">
									<strong>
										<xsl:value-of select="Approach"/>
									</strong>
								</td>
								<td class="approach">
									<strong>
										<xsl:value-of select="$cntApproach"/>
									</strong>
								</td>
							</tr>
						<!--XXX-->
						<!--<xsl:key name="keySurgeryType_Approach" match="/dsSchema/tblCategory" use="concat(SurgeryType,Approach)"/>-->
							<xsl:for-each select="key('keySurgeryType_Approach', concat($SurgeryType,$Approach))">
								<!--<xsl:if test="($SurgeryType = SurgeryType) and ($Approach = Approach)">-->
									<xsl:variable name="Category" select="Category"/>
									<xsl:variable name ="cntCategory" select="count(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach) and (Category = $Category)])"/>
									<tr>
										<td>&#160;</td>
										<td style="width:15px">&#160;</td>
										<td style="width:323px">
											<xsl:value-of select="Category_Desc"/>
										</td>
										<td>
											<strong>
												<xsl:value-of select="$cntCategory"/>
											</strong>
										</td>
									</tr>
								<!--</xsl:if>-->
							</xsl:for-each>
						<!--</xsl:if>-->
					</xsl:for-each>
				</xsl:for-each>
			</table>


			<img src="../../img/banner_sml.gif" class="smlBanner"/>
		</div>
    </body>
</html>
</xsl:template>

<xsl:template name="CheckMonths">
	<xsl:param name="MonthNo" />
	<xsl:choose>
		<xsl:when test="$MonthNo = 1">
		</xsl:when>
		<xsl:when test="$MonthNo = 3">
			1 - 3 Months
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$MonthNo"/>&#160;
			<xsl:choose>
				<xsl:when test="$MonthNo &gt; 1">months</xsl:when>
				<xsl:otherwise >month</xsl:otherwise>
			</xsl:choose>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="MakePatientDetailRow">
	<xsl:param name="MonthNo"/>

	<xsl:for-each select="key('keyMonth', $MonthNo)">
		<xsl:sort data-type="number" order="ascending" select="PatientID"/>
		<tr>
			<xsl:choose>
				<xsl:when test="position() mod 2 = 0">
					<xsl:attribute name="class">row01</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">row02</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td >
				<strong>
					<xsl:value-of select="PatientID"/>
				</strong>
			</td>
			<td >
				<xsl:value-of select="PatientName"/>
			</td>
			<td >
				<xsl:value-of select="AGE"/>
			</td>
			<td >
				<xsl:value-of select="LapbandDate"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="StartWeight"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="Weight"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="format-number(StartWeight - Weight, '##.00')"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="InitBMI"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="BMI"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="format-number(BMI - InitBMI, '##.00')"/>
			</td>
			<td style="text-align:right" >
				<xsl:value-of select="EWLL"/>
			</td>
		</tr>
	</xsl:for-each>
</xsl:template>

<xsl:template name="MakeEWLFooterData">
	<xsl:param name="Months"/>

	<xsl:variable name ="cntVisit" select="count(key('keyMonth', $Months)/DateSeen)"/>
	<xsl:variable name ="sumStartWeight" select="sum(key('keyMonth', $Months)/StartWeight)"/>
	<xsl:variable name ="sumWeight" select="sum(key('keyMonth', $Months)/Weight)"/>
	<xsl:variable name ="sumInitBMI" select="sum(key('keyMonth', $Months)/InitBMI)"/>
	<xsl:variable name ="sumBMI" select="sum(key('keyMonth', $Months)/BMI)"/>
	<xsl:variable name ="sumEWLL" select="sum(key('keyMonth', $Months)/EWLL)"/>
	<xsl:variable name ="EWL_LT_25" select ="count(key('keyMonth', $Months)[EWLL &lt; 25])"/>
	<xsl:variable name ="EWL_LT_50" select ="count(key('keyMonth', $Months)[(EWLL &gt;= 25) and (EWLL &lt;= 50)])"/>
	<xsl:variable name ="EWL_LT_75" select ="count(key('keyMonth', $Months)[(EWLL &gt; 50) and (EWLL &lt;= 75)])"/>
	<xsl:variable name ="EWL_LT_100" select ="count(key('keyMonth', $Months)[EWLL &gt; 75])"/>
	<xsl:variable name ="cntPatient" select="count(key('keyMonthPatientID', $Months))"/>
	<xsl:variable name = "sumAge" select="sum(key('keyMonthPatientID', $Months)/Age)"/>
	<tr>
		<td>
			<table >
				<tr>
					<td style="width:156px">
						<h4>
							<xsl:call-template name="CheckMonths" >
								<xsl:with-param name="MonthNo" select="$Months"/>
							</xsl:call-template>
						</h4>
					</td>
					<td style="width:100px">
						<strong>Average</strong>
					</td>
					<td style="text-align:center;width:40px">
						<strong>
							<xsl:value-of select="format-number($sumAge div $cntPatient, '##')"/>
						</strong>
					</td>
					<td align="center" width="80">
						<strong>
							<xsl:value-of select="format-number($sumStartWeight div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="60">
						<strong>
							<xsl:value-of select="format-number($sumWeight div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="50">
						<strong>
							<xsl:value-of select="format-number(($sumStartWeight - $sumWeight) div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="70">
						<strong>
							<xsl:value-of select="format-number($sumInitBMI div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="45">
						<strong>
							<xsl:value-of select="format-number($sumBMI div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="85">
						<strong>
							<xsl:value-of select="format-number(($sumInitBMI - $sumBMI) div $cntVisit, '##.00')"/>
						</strong>
					</td>
					<td align="center" width="50">
						<strong>
							<xsl:value-of select="format-number($sumEWLL div $cntVisit, '##.00')"/>
						</strong>
					</td>
				</tr>
				<tr>
					<td>
						<xsl:value-of select="$cntPatient"/>
						<xsl:choose >
							<xsl:when test="$cntPatient = 0"></xsl:when>
							<xsl:when test="$cntPatient = 1">Patient</xsl:when>
							<xsl:otherwise>Patients</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>&#160;</td>
					<td class="highlight" align="center">Age</td>
					<td class="highlight" align="center">
						Start Weight (<xsl:value-of select="/dsSchema/tblPatientList/WeightMeasurment"/>)
					</td>
					<td class="highlight" align="center">
						Weight (<xsl:value-of select="/dsSchema/tblPatientList/WeightMeasurment"/>)
					</td>
					<td class="highlight" align="center">Loss</td>
					<td class="highlight" align="center">Intial BMI</td>
					<td class="highlight" align="center">BMI</td>
					<td class="highlight" align="center">BMI Change</td>
					<td class="highlight" align="center">%EWL</td>
				</tr>
				<tr>
					<td>&#160;</td>
					<td class="greyTxt">S. Deviation</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="AgeSTDev != 'NaN'">
							<xsl:value-of select ="format-number(AgeSTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="StartWeightSTDev != 'NaN'">
							<xsl:value-of select ="format-number(StartWeightSTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="WeightSTDev != 'NaN'">
							<xsl:value-of select ="format-number(WeightSTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="WeightLossSTDev != 'NaN'">
							<xsl:value-of select ="format-number(WeightLossSTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="InitBMISTDev != 'NaN'">
							<xsl:value-of select ="format-number(InitBMISTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="BMISTDev != 'NaN'">
							<xsl:value-of select ="format-number(BMISTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="BMIChangeSTDev != 'NaN'">
							<xsl:value-of select ="format-number(BMIChangeSTDev, '##.0')"/>
						</xsl:if>
					</td>
					<td class="greyTxt" align="center">
						<xsl:if test ="EWLLSTDev != 'NaN'">
							<xsl:value-of select ="format-number(EWLLSTDev, '##.0')"/>
						</xsl:if>
					</td>
				</tr>

				<tr>
					<td>&#160;</td>
					<td>&#160;</td>
					<td colspan="8" align="center">
						<table class="ewl" border="0" width="100%">
							<tbody>
								<tr>
									<td align="center" width="25%">
										<strong>EWL&lt;25 : </strong>
										<xsl:value-of select="format-number(($EWL_LT_25 div $cntPatient) , '##.0')"/>
										<!--*100-->
										<!--$cntVisit-->
									</td>
									<td align="center" width="25%">
										<strong>EWL 25-50 : </strong>
										<xsl:value-of select="format-number(($EWL_LT_50 div $cntPatient) , '##.0')"/>
									</td>
									<td align="center" width="25%">
										<strong>EWL50-75 : </strong>
										<xsl:value-of select="format-number(($EWL_LT_75 div $cntPatient) , '##.0')"/>
									</td>
									<td align="center" width="25%">
										<strong>EWL &gt;75 : </strong>
										<xsl:value-of select="format-number(($EWL_LT_100 div $cntPatient) , '##.0')"/>
									</td>
								</tr>
							</tbody>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</xsl:template>

<xsl:template name="Followupdata">
	<xsl:param name="Title"/>
	<xsl:param name="FollowedUp"/>
	<xsl:param name="OperatedOn"/>

	<tr>
		<td>
			<strong>
				<xsl:value-of select="$Title"/>
			</strong>
		</td>
		<td style="text-align:center">
			<xsl:value-of select="$FollowedUp"/>
		</td>
		<td style="text-align:center">
			<xsl:value-of select="$OperatedOn"/>
		</td>
	</tr>
</xsl:template>
</xsl:stylesheet> 

