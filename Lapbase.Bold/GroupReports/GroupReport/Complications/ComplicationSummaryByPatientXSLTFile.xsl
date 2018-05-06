<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >

<xsl:key name="keyCompType" match="/dsSchema/tblDoctors" use="CompType"/>
<xsl:key name="keyCompType_SurgeonId" match="/dsSchema/tblSurgery" use="concat(CompType,SurgeonId)" />
<xsl:key name="keyCompType_SurgeonId_SurgeryType" match ="/dsSchema/tblComplication_Details" use="concat(CompType,SurgeonId,SurgeryType)"/>
<xsl:key name="keyCompType_SurgeonId_SurgeryType_ComplicationCode" match="/dsSchema/tblComplicationSummary" use="concat(CompType,SurgeonId,SurgeryType,ComplicationCode)"/>
<xsl:template match="/">
	<html>
		<head>
			<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
		</head>
		<body class="printPreview">
			<div class="groupComplicationsReport">
				<img src="../../img/print_header_bar.gif" height="8" width="800px" />
				<div class="clr"></div>

				<xsl:for-each select="/dsSchema/tblComplication">
					<xsl:variable name ="CompType" select="CompType"/>
					<table class ="details" border="0" >
						<tr>
							<td style="width:89px">
								<strong>Surgeon:</strong>
							</td>
							<td style="width:376px">
								<xsl:value-of select='translate(/dsSchema/tblComplicationSummary/DoctorName_Title, "`", "&apos;")'/>
							</td>
							<td rowspan="4" style="width:313px">
								<h2>
									<xsl:value-of select="CompType"/>
								</h2>
								<p class="printDate">
									<xsl:value-of select="ReportDate"/>
								</p>
							</td>
						</tr>
						<tr>
							<td>
								<strong>Hospital:</strong>
							</td>
							<td>
								<xsl:value-of select="/dsSchema/tblComplicationSummary/HospitalName_Title"/>
							</td>
						</tr>

						<tr>
							<td>
								<strong>Surgery:</strong>
							</td>
							<td>
								<!--<xsl:value-of select="dsSchema/tblComplicationSummary/SurgeryType_Desc"/>-->
							</td>
						</tr>

						<tr>
							<td>&#160;</td>
							<td>&#160;</td>
						</tr>
					</table>

					<!--<xsl:for-each select="/dsSchema/tblDoctors">
						<xsl:if test="$CompType = CompType">-->
					<xsl:for-each select="key('keyCompType', $CompType )">
							<xsl:variable name ="SurgeonId" select="SurgeonId"/>
							<table class="data" border="0" style="page-break-after:always;media:print">
								<tr>
									<th colspan="7">
										<h3>
											<xsl:value-of select='translate(DoctorName, "`", "&apos;")'/>
										</h3>
									</th>
								</tr>
								<xsl:for-each select="key('keyCompType_SurgeonId', concat($CompType,$SurgeonId))">
								<!--<xsl:for-each select="/dsSchema/tblSurgery">
									<xsl:if test="($CompType = CompType) and ($SurgeonId = SurgeonId)">-->
									<xsl:variable select="SurgeryType" name ="SurgeryType"/>
									<tr>
										<td colspan="7" >
											<h3>
												<xsl:value-of select="SurgeryType_Desc"/>
											</h3>
										</td>
									</tr>
									<tr>
										<td class="tblHeader" style="width:104px">Surgery</td>
										<td class="tblHeader" style="width:96px">Complication</td>
										<td class="tblHeader" style="width:100px">Weeks</td>
										<td class="tblHeader" style="width:89px"></td>
										<td class="tblHeader" style="width:60px"></td>
										<td class="tblHeader" style="width:100px"></td>
										<td class="tblHeader" style="width:205px"></td>
									</tr>
									<!--<xsl:key name="keyCompType_SurgeonId_SurgeryType" match ="/dsSchema/tblComplication_Details" use="concat(CompType,SurgeonId,SurgeryType)"/>-->
									<xsl:for-each select="key('keyCompType_SurgeonId_SurgeryType', concat($CompType,$SurgeonId,$SurgeryType))">
									<!--
									<xsl:for-each select="/dsSchema/tblComplication_Details">
										<xsl:if test ="($CompType = CompType) and ($SurgeonId = SurgeonId) and ($SurgeryType = SurgeryType)">-->
											<xsl:variable name ="ComplicationCode" select="ComplicationCode"/>
											<xsl:variable name ="cntOfComplication" select="count(/dsSchema/tblComplicationSummary[($CompType = CompType) and ($SurgeonId = SurgeonId) and ($SurgeryType = SurgeryType) and ($ComplicationCode = ComplicationCode)]) "/>
											<tr>
												<td colspan="3" class="tblSectionHeader">
													<h4>
														<xsl:value-of select="Complication" />
													</h4>
												</td>
												<td class="tblSectionHeader">
													Number <xsl:value-of select="$cntOfComplication"/>
												</td>
												<td class="tblSectionHeader"></td>
												<td class="tblSectionHeader"></td>
												<td class="tblSectionHeader"></td>
											</tr>
										<!-- <xsl:key name="keyCompType_SurgeonId_SurgeryType_ComplicationCode" match="/dsSchema/tblComplicationSummary" use="concat(CompType,SurgeonId,SurgeryType,ComplicationCode)"/>-->
										<!--
											<xsl:for-each select="/dsSchema/tblComplicationSummary">
												<xsl:if test ="($CompType = CompType) and ($SurgeonId = SurgeonId) and ($SurgeryType = SurgeryType) and ($ComplicationCode = ComplicationCode)">-->
										<xsl:for-each select="key('keyCompType_SurgeonId_SurgeryType_ComplicationCode', concat($CompType,$SurgeonId,$SurgeryType,$ComplicationCode))">
											<tr>
												<td colspan="4" bgcolor="#eaf1f9">
													<strong>
														<xsl:value-of select ="PatientName"/>
													</strong>
												</td>
												<td bgcolor="#eaf1f9">
													<strong>Sex:</strong>
													<xsl:value-of select ="Sex"/>
												</td>
												<td bgcolor="#eaf1f9">
													<strong>Age:</strong>
													<xsl:value-of select ="Age"/>
												</td>
												<td bgcolor="#eaf1f9">
													<strong>Surgery Date:</strong>
													<xsl:value-of select ="OperationDate"/>
												</td>
											</tr>
											<tr>
												<td>
													<strong>Date:</strong>
													<xsl:value-of select="ComplicationDate"/>
												</td>
												<td></td>
												<td>
													<strong>
														<xsl:value-of select="Weeks"/> weeks
													</strong>
												</td>
												<td colspan="4">
													<xsl:value-of select="ComplicationNotes"/>
												</td>
											</tr>
											<!--</xsl:if>-->
										</xsl:for-each>
										<!--</xsl:if>-->
									</xsl:for-each>
									<!--</xsl:if>-->
								</xsl:for-each>
							</table>
						<!--</xsl:if>-->
					</xsl:for-each>
				</xsl:for-each>
			</div>
		</body>
    </html>
</xsl:template>

</xsl:stylesheet> 

