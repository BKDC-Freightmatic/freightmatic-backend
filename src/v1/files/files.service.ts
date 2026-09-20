import { Injectable } from '@nestjs/common';
import * as fs from 'fs';
import * as path from 'path';
import { v4 as uuidv4 } from 'uuid';

@Injectable()
export class FilesService {
  private readonly uploadDir = path.join(process.cwd(), 'uploads');

  constructor() {
    if (!fs.existsSync(this.uploadDir)) {
      fs.mkdirSync(this.uploadDir, { recursive: true });
    }
  }

  async uploadFile(file: Express.Multer.File): Promise<{ title: string; url: string }> {
    const ext = path.extname(file.originalname);
    const fileName = `${uuidv4()}${ext}`;
    const filePath = path.join(this.uploadDir, fileName);

    await fs.promises.writeFile(filePath, file.buffer);

    const baseUrl = process.env.BASE_URL || 'http://localhost:4000';
    return {
      title: file.originalname,
      url: `${baseUrl}/uploads/${fileName}`,
    };
  }

  async uploadFiles(files: Express.Multer.File[]): Promise<{ title: string; url: string }[]> {
    const results = [];
    for (const file of files) {
      const res = await this.uploadFile(file);
      results.push(res);
    }
    return results;
  }
}
