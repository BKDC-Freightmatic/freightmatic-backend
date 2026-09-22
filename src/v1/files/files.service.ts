import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { existsSync, mkdirSync } from 'fs';
import { join } from 'path';
import { FileEntity } from './entities/file.entity';

@Injectable()
export class FilesService {
  private readonly uploadDir = join(process.cwd(), 'uploads');

  constructor(
    @InjectRepository(FileEntity)
    private readonly fileRepository: Repository<FileEntity>,
  ) {
    if (!existsSync(this.uploadDir)) {
      mkdirSync(this.uploadDir, { recursive: true });
    }
  }

  async saveFile(
    fileBuffer: Buffer,
    originalName: string,
    hostUrl: string,
  ): Promise<{ title: string; url: string }> {
    // Sanitize file name to remove spaces and special characters for clean URLs
    const sanitizedName = originalName.replace(/\s+/g, '_').replace(/[^a-zA-Z0-9._-]/g, '');
    const uniqueFilename = `${Date.now()}-${sanitizedName}`;
    const filePath = join(this.uploadDir, uniqueFilename);

    // Save to disk
    await require('fs').promises.writeFile(filePath, fileBuffer);

    // Construct URL dynamically
    const fileUrl = `${hostUrl}/uploads/${encodeURIComponent(uniqueFilename)}`;

    // Create DB entry
    const fileEntity = this.fileRepository.create({
      filename: uniqueFilename,
      originalName: originalName,
      path: filePath,
      url: fileUrl,
    });
    await this.fileRepository.save(fileEntity);

    return {
      title: originalName,
      url: fileUrl,
    };
  }

  async uploadFiles(
    files: Express.Multer.File[],
    hostUrl: string,
  ): Promise<{ title: string; url: string }[]> {
    const results = [];
    for (const file of files) {
      const res = await this.saveFile(file.buffer, file.originalname, hostUrl);
      results.push(res);
    }
    return results;
  }
}
