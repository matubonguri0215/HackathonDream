using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HPParticle : MonoBehaviour
{
    public Transform target;
    public float ãzà¯óÕ = 5f;

    public float trailLength = 0.3f;
    public float trailWidth = 0.08f;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        // Trailê›íËÇÉXÉNÉäÉvÉgÇ≈
        var trails = ps.trails;
        trails.enabled = true;
        trails.mode = ParticleSystemTrailMode.PerParticle;
        trails.lifetime = trailLength;

        var width = trails.widthOverTrail;
        width.mode = ParticleSystemCurveMode.TwoConstants;
        width.constantMin = trailWidth;
        width.constantMax = 0f;
        trails.widthOverTrail = width;

        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        if (target == null) return;

        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = target.position - particles[i].position;
            float dist = dir.magnitude;

            // ãzé˚â¡ë¨
            particles[i].velocity +=
                dir.normalized
                * ãzà¯óÕ
                * Time.deltaTime
                * (1f + 1f / Mathf.Max(dist, 0.1f));
        }

        ps.SetParticles(particles, count);
    }
}
