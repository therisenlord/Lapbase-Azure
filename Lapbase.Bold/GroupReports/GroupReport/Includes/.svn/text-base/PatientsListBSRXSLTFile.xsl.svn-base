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
                  <td style="width:143px">
                    <xsl:value-of select="Surname"/>
                  </td>
                  <td style="width:90px">
                    <xsl:value-of select="Firstname"/>
                  </td>
                  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="1"/>
                      <xsl:with-param name="value" select ="Form1"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>                    
                    <xsl:value-of select="Form1"/>
								  </td>
								  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="2"/>
                      <xsl:with-param name="value" select ="Form2"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Form2"/>
								  </td>
								  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="3"/>
                      <xsl:with-param name="value" select ="Form3"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form3"/>
								  </td>
								  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="4"/>
                      <xsl:with-param name="value" select ="Form4"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form4"/>
								  </td>
                  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="5"/>
                      <xsl:with-param name="value" select ="Form5"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form5"/>
                  </td>
								  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="6"/>
                      <xsl:with-param name="value" select ="Form6"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form6"/>
                  </td>
                  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="7"/>
                      <xsl:with-param name="value" select ="Form7"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form7"/>
                  </td>
                  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="8"/>
                      <xsl:with-param name="value" select ="Form8"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form8"/>
                  </td>
                  <td style="width:63px">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="9"/>
                      <xsl:with-param name="value" select ="Form9"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                    </xsl:call-template>          
                    <xsl:value-of select="Form9"/>
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

<xsl:template name = "fnClickable">
  <xsl:param name ="form" ></xsl:param>
  <xsl:param name ="value" ></xsl:param>
  <xsl:param name ="patientid" ></xsl:param>
  <xsl:choose>
    <xsl:when test ="$value = 'Available'">
      <xsl:attribute name="style">
        color:green;width:63px
      </xsl:attribute>
      <xsl:attribute name="onclick">
        javascript:ACSForm_onclick(<xsl:value-of select="$form"/>, '<xsl:value-of select="$value"/>', <xsl:value-of select="$patientid"/>);
      </xsl:attribute>
      <xsl:attribute name="onmouseover">
        javascript:this.style.cursor='pointer';
      </xsl:attribute>
      <xsl:attribute name="onmouseout">
        javascript:this.style.cursor='';
      </xsl:attribute>
    </xsl:when>
    <xsl:when test ="$value = 'Viewed'">
      <xsl:attribute name="style">
        color:blue;width:63px
      </xsl:attribute>
      <xsl:attribute name="onclick">
        javascript:ACSForm_onclick(<xsl:value-of select="$form"/>, '<xsl:value-of select="$value"/>', <xsl:value-of select="$patientid"/>);
      </xsl:attribute>
      <xsl:attribute name="onmouseover">
        javascript:this.style.cursor='pointer';
      </xsl:attribute>
      <xsl:attribute name="onmouseout">
        javascript:this.style.cursor='';
      </xsl:attribute>
    </xsl:when>
    <xsl:otherwise>
      <xsl:attribute name="style">
        color:red;width:63px
      </xsl:attribute>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
  
</xsl:stylesheet> 

