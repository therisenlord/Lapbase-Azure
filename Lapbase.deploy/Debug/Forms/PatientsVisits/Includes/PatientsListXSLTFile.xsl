<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
		<!--<div class="visitDisplayScroll" >-->
			  <table style="width:895px" cellpadding = "0" cellspacing = "0" border="0" class="testNameTable">
				<xsl:for-each select="dsSchema/tblPatients">
				  <tr>
					  <td>
						  <xsl:if test='position() mod 2 = 1'>
							  <xsl:attribute name="class">row01</xsl:attribute>
						  </xsl:if>
						  <xsl:if test='position() mod 2 != 1'>
							  <xsl:attribute name="class">row02</xsl:attribute>
						  </xsl:if>
						  <table style="width:90%" cellpadding = "0" cellspacing = "0" border="0">
							  <tr>
								  <xsl:attribute name="onclick">
									  javascript:PatientRow_onclick(<xsl:value-of select="PatientID"/>);
								  </xsl:attribute>
								  <xsl:attribute name="onmouseover">
									  javascript:this.style.cursor='pointer';
								  </xsl:attribute>
								  <xsl:attribute name="onmouseout">
									  javascript:this.style.cursor='';
								  </xsl:attribute>
								  <td style="width:90px">
									  <xsl:choose>
										  <xsl:when test ="string-length(Patient_CustomId) > 0">
											  <xsl:value-of select="Patient_CustomId"/>
										  </xsl:when>
										  <xsl:otherwise>
											  <xsl:value-of select="PatientID"/>
										  </xsl:otherwise>
									  </xsl:choose>
								  </td>
								  <td style="width:145px">
									  <xsl:value-of select="Surname"/>
								  </td>
								  <td style="width:85px">
									  <xsl:value-of select="Firstname"/>
								  </td>
								  <td style="width:90px">
									  <xsl:value-of select="Suburb"/>
								  </td>
								  <td style="width:80px">
									  <xsl:value-of select="tempBirthdate"/>
								  </td>
								  <td style="width:80px">
									  <xsl:value-of select="tempLapBandDate"/>
								  </td>
								  <td style="width:175px">
									  <xsl:value-of select="SurgeryType_Description"/>
								  </td>
                  <td style="width:55px">
                    <xsl:value-of select="DoctorInitial"/>
                  </td>
								  <td style="width:80px">
									  <xsl:value-of select="tempDateLastVisit"/>
								  </td>
							  </tr>
						  </table>
					  </td>
				  </tr>
				</xsl:for-each>
			  </table>
		<!--</div>-->
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

