using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class QualityDefectServices : IQualityDefectServices
    {
        private readonly IQualityDefectRepository qualitydefectRepository;
        public QualityDefectServices(IQualityDefectRepository _qualitydefectRepository)
        {
            qualitydefectRepository = _qualitydefectRepository;
        }

        public List<QualityDefectViewModel> getAllQualityDefects()
        {
            return qualitydefectRepository.getAllQualityDefects();
        }


        public int CreateQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            return qualitydefectRepository.CreateQualityDefect(eng_qa_defect, path, halfPath);
        }

        public int SaveQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            return qualitydefectRepository.SaveQualityDefect(eng_qa_defect, path, halfPath);
        }
        public int DeleteQualityDefect(int qualitydefectID)
        {
            return qualitydefectRepository.DeleteQualityDefect(qualitydefectID);
        }

        public QualityDefectViewModel getQualityDefect(int qualitydefectID)
        {

            return qualitydefectRepository.getQualityDefect(qualitydefectID);
        }

        public QualityDefectDetailViewModel getQualityDefectDetail(int? id)
        {
            return qualitydefectRepository.getQualityDefectDetail(id);
        }

        
        public int deleteQualityDefectDetail(int? id)
        {
            return qualitydefectRepository.deleteQualityDefectDetail(id);
        }
        public int deleteQualityDefectFile(int? id)
        {
            return qualitydefectRepository.deleteQualityDefectFile(id);
        }
        public int ApproveRejectQualityDefect(QualityDefectViewModel qualitydefect)
        {
            return qualitydefectRepository.ApproveRejectQualityDefect(qualitydefect);
        }
        public List<QualityDefectDetailViewModel> getAllDefectDetails()
        {
            return qualitydefectRepository.getAllDefectDetails();
        }
        public QualityDefectCPAViewModel getQualityCPADetail(int? id)
        {
            return qualitydefectRepository.getQualityCPADetail(id);
        }
        public int AddEditCpa(QualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath)
        {
            return qualitydefectRepository.AddEditCpa(eng_qa_defect_cpa, path, halfPath);
        }
        public int deleteQualityCPAFile(int? id)
        {
            return qualitydefectRepository.deleteQualityCPAFile(id);
        }
        public int CreateQualityDefect(MobileQualityDefectViewModel eng_qa_defect, string path, string halfPath)
        {
            return qualitydefectRepository.CreateQualityDefect(eng_qa_defect, path, halfPath);
        }

        public int AddEditCpa(MobileQualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath)
        {
            return qualitydefectRepository.AddEditCpa(eng_qa_defect_cpa, path, halfPath);
        }

    }
}