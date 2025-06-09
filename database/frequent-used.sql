SELECT id,
       title,
       assignee_id,
       project_id,
       status,
       last_modified
FROM public.tasks
LIMIT 1000;

-- indexing column
CREATE INDEX idx_title ON tasks(title);

-- check indexing
SELECT indexname, indexdef
FROM pg_indexes
WHERE tablename = 'tasks';