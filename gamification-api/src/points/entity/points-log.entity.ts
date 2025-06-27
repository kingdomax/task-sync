import {
    Entity,
    PrimaryGeneratedColumn,
    Column,
    CreateDateColumn,
} from 'typeorm';

@Entity('points_log')
export class PointsLogEntity {
    @PrimaryGeneratedColumn()
    id: number;

    @Column()
    user_id: number;

    @Column({ nullable: true })
    task_id: number;

    @Column()
    points_awarded: number;

    @Column()
    reason: string;

    @CreateDateColumn({ type: 'timestamp' })
    created_at: Date;
}
